import numpy as np
import pandas as pd
from numba import njit
from itertools import product
from ortools.linear_solver import pywraplp
import csv
from os import path

#######################################################################
def get_penalty(n, choice):
    penalty = None
    if choice == 0:
        penalty = 0
    elif choice == 1:
        penalty = 50
    elif choice == 2:
        penalty = 50 + 9 * n
    elif choice == 3:
        penalty = 100 + 9 * n
    elif choice == 4:
        penalty = 200 + 9 * n
    elif choice == 5:
        penalty = 200 + 18 * n
    elif choice == 6:
        penalty = 300 + 18 * n
    elif choice == 7:
        penalty = 300 + 36 * n
    elif choice == 8:
        penalty = 400 + 36 * n
    elif choice == 9:
        penalty = 500 + 36 * n + 199 * n
    else:
        penalty = 500 + 36 * n + 398 * n
    return penalty


def GetPreferenceCostMatrix(data):
    cost_matrix = np.zeros((N_FAMILIES, N_DAYS), dtype=np.int64)
    for i in range(N_FAMILIES):
        desired = data.values[i, :-1]
        cost_matrix[i, :] = get_penalty(FAMILY_SIZE[i], 10)
        for j, day in enumerate(desired):
            cost_matrix[i, day-1] = get_penalty(FAMILY_SIZE[i], j)
    return cost_matrix


def GetAccountingCostMatrix():
    ac = np.zeros((1000, 1000), dtype=np.float64)
    for n in range(ac.shape[0]):
        for n_p1 in range(ac.shape[1]):
            diff = abs(n - n_p1)
            ac[n, n_p1] = max(0, (n - 125) / 400 * n**(0.5 + diff / 50.0))
    return ac


# preference cost
@njit(fastmath=True)
def pcost(prediction):
    daily_occupancy = np.zeros(N_DAYS+1, dtype=np.int64)
    penalty = 0
    for (i, p) in enumerate(prediction):
        n = FAMILY_SIZE[i]
        penalty += PCOSTM[i, p]
        daily_occupancy[p] += n
    return penalty, daily_occupancy


# accounting cost
@njit(fastmath=True)
def acost(daily_occupancy):
    accounting_cost = 0
    n_out_of_range = 0
    daily_occupancy[-1] = daily_occupancy[-2]
    for day in range(N_DAYS):
        n_p1 = daily_occupancy[day + 1]
        n    = daily_occupancy[day]
        n_out_of_range += (n > MAX_OCCUPANCY) or (n < MIN_OCCUPANCY)
        accounting_cost += ACOSTM[n, n_p1]
    return accounting_cost, n_out_of_range


@njit(fastmath=True)
def cost_function(prediction):
    penalty, daily_occupancy = pcost(prediction)
    accounting_cost, n_out_of_range = acost(daily_occupancy)
    return penalty + accounting_cost + n_out_of_range*100000000


#######################################################################
def findBetterDay4Family(pred):
    fobs = np.argsort(FAMILY_SIZE)
    score = cost_function(pred)
    original_score = np.inf

    while original_score > score:
        original_score = score
        for family_id in fobs:
            for pick in range(10):
                day = DESIRED[family_id, pick]
                oldvalue = pred[family_id]
                pred[family_id] = day
                new_score = cost_function(pred)
                if new_score < score:
                    score = new_score
                else:
                    pred[family_id] = oldvalue

        print(score, end='\r')
    print(score)


def stochastic_product_search(top_k, fam_size, original,
                              verbose=1000, verbose2=50000,
                              n_iter=500, random_state=2019):
    """
    original (np.array): The original day assignments.

    At every iterations, randomly sample fam_size families. Then, given their top_k
    choices, compute the Cartesian product of the families' choices, and compute the
    score for each of those top_k^fam_size products.
    """

    best = original.copy()
    best_score = cost_function(best)

    np.random.seed(random_state)

    for i in range(n_iter):
        fam_indices = np.random.choice(range(DESIRED.shape[0]), size=fam_size)
        changes = np.array(list(product(*DESIRED[fam_indices, :top_k].tolist())))

        for change in changes:
            new = best.copy()
            new[fam_indices] = change

            new_score = cost_function(new)

            if new_score < best_score:
                best_score = new_score
                best = new

        if verbose and i % verbose == 0:
            print(f"Iteration #{i}: Best score is {best_score:.2f}      ", end='\r')

        if verbose2 and i % verbose2 == 0:
            print(f"Iteration #{i}: Best score is {best_score:.2f}      ")

    print(f"Final best score is {best_score:.2f}")
    return best



#######################################################################
def seed_finding(seed, prediction_input):
    prediction = prediction_input.copy()
    np.random.seed(seed)
    best_score = cost_function(prediction)
    original_score = best_score
    best_pred = prediction.copy()
    print("SEED: {}   ORIGINAL SCORE: {}".format(seed, original_score))
    for t in range(100):
        for i in range(5000):
            for j in range(10):
                di = prediction[i]
                prediction[i] = DESIRED[i, j]
                cur_score = cost_function(prediction)

                KT = 1
                if t < 5:
                    KT = 1.5
                elif t < 10:
                    KT = 4.5
                else:
                    if cur_score > best_score + 100:
                        KT = 3
                    elif cur_score > best_score + 50 :
                        KT = 2.75
                    elif cur_score > best_score + 20:
                        KT = 2.5
                    elif cur_score > best_score + 10:
                        KT = 2
                    elif cur_score > best_score:
                        KT = 1.5
                    else:
                        KT = 1

                prob = np.exp(-(cur_score - best_score) / KT)
                if np.random.rand() < prob:
                    best_score = cur_score
                else:
                    prediction[i] = di
        if best_score < original_score:
            print("NEW BEST SCORE on seed {}: {}".format(seed, best_score))
            original_score = best_score
            best_pred = prediction.copy()

    return prediction




#######################################################################
def solveSantaLP():
    S = pywraplp.Solver('SolveAssignmentProblem', pywraplp.Solver.GLOP_LINEAR_PROGRAMMING)

    # S.SetNumThreads(NumThreads)
    # S.set_time_limit(limit_in_seconds*1000*NumThreads) #cpu time = wall time * N_threads

    x = {}
    candidates = [[] for _ in range(N_DAYS)]  # families that can be assigned to each day

    for i in range(N_FAMILIES):
        for j in DESIRED[i, :]:
            candidates[j].append(i)
            x[i, j] = S.BoolVar('x[%i,%i]' % (i, j))

    daily_occupancy = [S.Sum([x[i, j] * FAMILY_SIZE[i] for i in candidates[j]])
                       for j in range(N_DAYS)]

    family_presence = [S.Sum([x[i, j] for j in DESIRED[i, :]])
                       for i in range(N_FAMILIES)]

    # Objective
    preference_cost = S.Sum([PCOSTM[i, j] * x[i, j] for i in range(N_FAMILIES)
                             for j in DESIRED[i, :]])

    S.Minimize(preference_cost)

    # Constraints
    for j in range(N_DAYS - 1):
        S.Add(daily_occupancy[j] - daily_occupancy[j + 1] <= 23)
        S.Add(daily_occupancy[j + 1] - daily_occupancy[j] <= 23)

    for i in range(N_FAMILIES):
        S.Add(family_presence[i] == 1)

    for j in range(N_DAYS):
        S.Add(daily_occupancy[j] >= MIN_OCCUPANCY)
        S.Add(daily_occupancy[j] <= MAX_OCCUPANCY)

    #f_days, c_days, n_days = load("best.csv")
    #res=c_days
    res = S.Solve()

    resdict = {0: 'OPTIMAL', 1: 'FEASIBLE', 2: 'INFEASIBLE', 3: 'UNBOUNDED',
               4: 'ABNORMAL', 5: 'MODEL_INVALID', 6: 'NOT_SOLVED'}

    print('LP solver result:', resdict[res])

    l = [(i, j, x[i, j].solution_value()) for i in range(N_FAMILIES)
         for j in DESIRED[i, :]
         if x[i, j].solution_value() > 0]

    df = pd.DataFrame(l, columns=['family_id', 'day', 'n'])
    return df


#######################################################################

def solveSantaIP(families, min_occupancy, max_occupancy):
    S = pywraplp.Solver('SolveAssignmentProblem', pywraplp.Solver.CBC_MIXED_INTEGER_PROGRAMMING)

    # S.SetNumThreads(NumThreads)
    # S.set_time_limit(limit_in_seconds*1000*NumThreads) #cpu time = wall time * N_threads

    n_families = len(families)

    x = {}
    candidates = [[] for _ in range(N_DAYS)]  # families that can be assigned to each day

    for i in families:
        for j in DESIRED[i, :]:
            candidates[j].append(i)
            x[i, j] = S.BoolVar('x[%i,%i]' % (i, j))

    daily_occupancy = [S.Sum([x[i, j] * FAMILY_SIZE[i] for i in candidates[j]])
                       for j in range(N_DAYS)]

    family_presence = [S.Sum([x[i, j] for j in DESIRED[i, :]])
                       for i in families]

    # Objective
    preference_cost = S.Sum([PCOSTM[i, j] * x[i, j] for i in families
                             for j in DESIRED[i, :]])

    S.Minimize(preference_cost)

    # Constraints

    for i in range(n_families):
        S.Add(family_presence[i] == 1)

    for j in range(N_DAYS):
        S.Add(daily_occupancy[j] >= min_occupancy[j])
        S.Add(daily_occupancy[j] <= max_occupancy[j])

    res = S.Solve()

    resdict = {0: 'OPTIMAL', 1: 'FEASIBLE', 2: 'INFEASIBLE', 3: 'UNBOUNDED',
               4: 'ABNORMAL', 5: 'MODEL_INVALID', 6: 'NOT_SOLVED'}

    print('MIP solver result:', resdict[res])

    l = [(i, j) for i in families
         for j in DESIRED[i, :]
         if x[i, j].solution_value() > 0]

    df = pd.DataFrame(l, columns=['family_id', 'day'])
    return df



#######################################################################

def solveSanta():
    df = solveSantaLP()  # Initial solution for most of families

    THRS = 0.999

    assigned_df = df[df.n > THRS].copy()
    unassigned_df = df[(df.n <= THRS) & (df.n > 1 - THRS)]
    unassigned = unassigned_df.family_id.unique()
    print('{} unassigned families'.format(len(unassigned)))

    assigned_df['family_size'] = FAMILY_SIZE[assigned_df.family_id]
    occupancy = assigned_df.groupby('day').family_size.sum().values
    min_occupancy = np.array([max(0, MIN_OCCUPANCY - o) for o in occupancy])
    max_occupancy = np.array([MAX_OCCUPANCY - o for o in occupancy])

    rdf = solveSantaIP(unassigned, min_occupancy, max_occupancy)  # solve the rest with MIP
    df = pd.concat((assigned_df[['family_id', 'day']], rdf)).sort_values('family_id')
    return df.day.values



#######################################################################

N_DAYS = 100
N_FAMILIES = 5000
MAX_OCCUPANCY = 300
MIN_OCCUPANCY = 125

data = pd.read_csv('family_data.csv', index_col='family_id')

FAMILY_SIZE = data.n_people.values
DESIRED     = data.values[:, :-1] - 1
PCOSTM = GetPreferenceCostMatrix(data) # Preference cost matrix
ACOSTM = GetAccountingCostMatrix()     # Accounting cost matrix



DBL_MAX = 1e+308

N_FAMILES = 5000
N_DAYS = 100
N_CHOICES = 10
MAX_OCCUPANCY = 300
MIN_OCCUPANCY = 125

MAX_DIFF = 300
MAX_DIFF2 = MAX_DIFF * 2
MAX_FAMILY_PER_DAY = 200

DATA_DIR = "C:/Users/крендель/Desktop/MagicCode/Машинное обучение/Santa's Workshop Tour 2019"
@njit(fastmath=True)
def build_cost_lut(family_size, family_choice):
    pref_cost = np.empty((N_FAMILES, N_DAYS), dtype=np.float64)
    acc1_cost = np.empty((MAX_DIFF2,), dtype=np.float64)
    acc_cost = np.empty((MAX_DIFF2, MAX_DIFF2), dtype=np.float64)
    penalty = np.empty((MAX_DIFF2,), dtype=np.float64)

    for i in range(N_FAMILES):
        # preference cost
        n = family_size[i]
        pref_cost[i][:] = 500 + 36 * n + 398 * n
        pref_cost[i][family_choice[i][0]] = 0
        pref_cost[i][family_choice[i][1]] = 50
        pref_cost[i][family_choice[i][2]] = 50 + 9 * n
        pref_cost[i][family_choice[i][3]] = 100 + 9 * n
        pref_cost[i][family_choice[i][4]] = 200 + 9 * n
        pref_cost[i][family_choice[i][5]] = 200 + 18 * n
        pref_cost[i][family_choice[i][6]] = 300 + 18 * n
        pref_cost[i][family_choice[i][7]] = 300 + 36 * n
        pref_cost[i][family_choice[i][8]] = 400 + 36 * n
        pref_cost[i][family_choice[i][9]] = 500 + 36 * n + 199 * n

    for i in range(MAX_DIFF2):
        # accounting cost
        acc1_cost[i] = max(0, (i - 125.0) / 400.0 * i ** 0.5)
        for j in range(MAX_DIFF2):
            diff = abs(j - MAX_DIFF)
            acc_cost[i][j] = max(0, (i - 125.0) / 400.0 * i ** (0.5 + diff / 50.0))

        # constraint penalty
        if i > MAX_OCCUPANCY:
            penalty[i] = 60 * (i - MAX_OCCUPANCY + 1) ** 1.2
        elif i < MIN_OCCUPANCY:
            penalty[i] = 60 * (MIN_OCCUPANCY - i + 1) ** 1.2
        else:
            penalty[i] = 0

    return pref_cost, acc1_cost, acc_cost, penalty


def build_global_data(data_dir):
    # family data
    family_choice = np.empty((N_FAMILES, N_CHOICES), dtype=np.int32)
    family_size = np.empty((N_FAMILES,), dtype=np.int32)

    with open(path.join(data_dir, "family_data.csv"), "r") as f:
        reader = csv.reader(f)
        next(reader, None)
        for row in reader:
            i = int(row[0])
            choices = [int(c) - 1 for c in row[1:N_CHOICES + 1]]
            members = int(row[N_CHOICES + 1])
            family_size[i] = members
            family_choice[i] = choices
    # cost lut
    pref_cost, acc1_cost, acc_cost, penalty = build_cost_lut(family_size, family_choice)
    return pref_cost, acc1_cost, acc_cost, penalty, family_choice, family_size
PREF_COST, ACC1_COST, ACC_COST, PENALTY, FAMILY_CHOICES, FAMILY_SIZE2 = build_global_data(DATA_DIR)
@njit
def day_insert(f_days, c_days, n_days, day, family_id):
    """ insert family_id to day
    """
    f_days[day][c_days[day]] = family_id
    c_days[day] += 1
    n_days[day] += FAMILY_SIZE2[family_id]

def load(filename):
    f_days = np.zeros((N_FAMILES, MAX_FAMILY_PER_DAY), dtype=np.int32)
    c_days = np.zeros((N_FAMILES,), dtype=np.int32)
    n_days = np.zeros((N_FAMILES,), dtype=np.int32)

    with open(filename, "r") as f:
        reader = csv.reader(f)
        next(reader, None)
        for row in reader:
            family_id, day = int(row[0]), int(row[1]) - 1
            day_insert(f_days, c_days, n_days, day, family_id)
    return f_days, c_days, n_days


#######################################################################

prediction = solveSanta()


pc, occ = pcost(prediction)
ac, _ = acost(occ)
print('{}, {:.2f}, ({}, {})'.format(pc, ac, occ.min(), occ.max()))


new = prediction.copy()
findBetterDay4Family(new)

#######################################################################
final = stochastic_product_search(
        top_k=2,
        fam_size=8,
        original=new,
        n_iter=500000,
        verbose=1000,
        verbose2=50000,
        random_state=2019
        )


final = seed_finding(2019, final)

sub = pd.DataFrame(range(N_FAMILIES), columns=['family_id'])
sub['assigned_day'] = final+1
sub.to_csv('submission.csv', index=False)