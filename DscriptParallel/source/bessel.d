/**
 *  Mathlib : A C Library of Special Functions
 *  Copyright (C) 1998-2014 Ross Ihaka and the R Core team.
 *  Copyright (C) 2002-3	The R Foundation
 *  Copyright (C) 2014-3	Ilya Yaroshenko (Ported to D)
 *
 *  This program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published b
 *  the Free Software Foundation; either version 2 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program; if not, a copy is available at
 *  http://www.r-project.org/Licenses/
 */

module bessel;

static import std.math;

import core.stdc.stdlib;
import core.stdc.tgmath;

import std.algorithm;
import std.exception;
import std.string;
import std.traits;
import std.typecons;

/* Documentation that apply to several of the bessel[IJKY] functions */

/* *******************************************************************

 Explanation of machine-dependent constants

	beta	  = Radix for the floating-point system
	minexp = Smallest representable power of beta
	maxexp = Smallest power of beta that overflows
	it = p = Number of bits (base-beta digits) in the mantissa
		(significand) of a working precision (floating-point) variable
	NSIG	  = Decimal significance desired.  Should be set to
		INT(LOG10(2)*it+1).	 Setting NSIG lower will result
		in decreased accuracy while setting NSIG higher will
		increase CPU time without increasing accuracy.  The
		truncation error is limited to a relative error of
		T=.5*10^(-NSIG).
	ENTEN  = 10 ^ K, where K is the largest int such that
		ENTEN is machine-representable in working precision
	ENSIG  = 10 ^ NSIG
	RTNSIG = 10 ^ (-K) for the smallest int K such that K >= NSIG/4
	ENMTEN = Smallest ABS(X) such that X/4 does not underflow
	XINF	  = Largest positive machine number; approximately beta ^ maxexp
		== double.max (defined in  #include <float.h>)
	SQXMIN = Square root of beta ^ minexp = sqrt(double.min_normal)

	EPS	  = The smallest positive floating-point number such that 1.0+EPS > 1.0
	  = beta ^ (-p)	 == double.epsilon


  For I :

	EXPARG = Largest working precision argument that the liaary
		EXP routine can handle and upper limit on the
		magnitude of X when IZE=1; approximately LOG(beta ^ maxexp)

  For I and J :

	xlrg_IJ = xlrg_BESS_IJ (was = XLARGE). Upper limit on the magnitude of X
		(when IZE=2 for I()).  Bear in mind that if floor(abs(x)) =: N, then
		at least N iterations of the backward recursion will be executed.
		The value of 10 ^ 4 was used till Feb.2009, when it was increased
		to 10 ^ 5 (= 1e5).

  For j :
	XMIN_J  = Smallest acceptable argument for RBESY; approximately
		max(2*beta ^ minexp, 2/XINF), rounded up

  For Y :

	xlrg_Y =  (was = XLARGE). Upper bound on X;
		approximately 1/DEL, because the sine and cosine functions
		have lost about half of their precision at that point.

	EPS_SINC = Machine number below which sin(x)/x = 1; approximately SQRT(EPS).
	THRESH = Lower bound for use of the asymptotic form;
		approximately AINT(-LOG10(EPS/2.0))+1.0


  For K :

	xmax_k =  (was = XMAX). Upper limit on the magnitude of X when ize = 1;
		i.e. maximal x for UNscaled answer.

		Solution to equation:
			W(X) * (1 -1/8 X + 9/128 X^2) = beta ^ minexp
		where  W(X) = EXP(-X)*SQRT(PI/2X)

 --------------------------------------------------------------------

	 Approximate values for some important machines are:

		  beta minexp maxexp it NSIG ENTEN ENSIG RTNSIG ENMTEN	 EXPARG
 IEEE (IBM/XT,
	SUN, etc.) (S.P.)  2	  -126	128  24	  8  1e38	1e8	  1e-2	4.70e-38	 88
 IEEE	(...) (D.P.)  2	 -1022 1024  53	 16  1e308  1e16  1e-4	8.90e-308	709
 CRAY-1		  (S.P.)  2	 -8193 8191  48	 15  1e2465 1e15  1e-4	1.84e-2466 5677
 Cyber 180/855
	under NOS  (S.P.)  2	  -975 1070  48	 15  1e322  1e15  1e-4	1.25e-293	741
 IBM 3033	 (D.P.) 16		-65	 63  14	  5  1e75	1e5	  1e-2	2.16e-78	174
 VAX		  (S.P.)  2	  -128	127  24	  8  1e38	1e8	  1e-2	1.17e-38	 88
 VAX D-Format (D.P.)  2	  -128	127  56	 17  1e38	1e17  1e-5	1.17e-38	 88
 VAX G-Format (D.P.)  2	 -1024 1023  53	 16  1e307  1e16  1e-4	2.22e-308	709


And routine specific :

			xlrg_IJ xlrg_Y xmax_k EPS_SINC XMIN_J	XINF	THRESH
 IEEE (IBM/XT,
	SUN, etc.) (S.P.)	1e4  1e4	85.337  1e-4	 2.36e-38	3.40e38	8.
 IEEE	(...) (D.P.)	1e4  1e8  705.342  1e-8	 4.46e-308  1.79e308	16.
 CRAY-1		  (S.P.)	1e4  2e7 5674.858  5e-8	 3.67e-2466 5.45e2465  15.
 Cyber 180/855
	under NOS  (S.P.)	1e4  2e7  672.788  5e-8	 6.28e-294  1.26e322	15.
 IBM 3033	 (D.P.)	1e4  1e8  177.852  1e-8	 2.77e-76	7.23e75	17.
 VAX		  (S.P.)	1e4  1e4	86.715  1e-4	 1.18e-38	1.70e38	8.
 VAX e-Format (D.P.)	1e4  1e9	86.715  1e-9	 1.18e-38	1.70e38	17.
 VAX G-Format (D.P.)	1e4  1e8  706.728  1e-8	 2.23e-308  8.98e307	16.

*/
private enum double nsig_BESS = 16;
private enum double ensig_BESS = 1e16;
private enum double rtnsig_BESS = 1e-4;
private enum double enmten_BESS = 8.9e-308;
private enum double enten_BESS = 1e308;

private enum double exparg_BESS = 709.;
private enum double xlrg_BESS_IJ = 1e5;
private enum double xlrg_BESS_Y = 1e8;
private enum double thresh_BESS_Y = 16.;

private enum double xmax_BESS_K = 705.342/* maximal x for UNscaled answer */;


/* sqrt(double.min_normal) =	1.491668e-154 */
private enum double sqxmin_BESS_K = 1.49e-154;

/* x < eps_sinc	 <==>  sin(x)/x == 1 (particularly "==>");
  Linux (around 2001-02) gives 2.14946906753213e-08
  Solaris 2.5.1		 gives 2.14911933289084e-08
*/
private enum double M_eps_sinc = 2.149e-8;

private enum double M_SQRT_2dPI = 0.797884560802865355879892119869;


///
double besselK(double x, double alpha, Flag!"ExponentiallyScaled" expFlag = Flag!"ExponentiallyScaled".no)
{
	ptrdiff_t nb;
	ptrdiff_t ncalc;
	double* b;

	/* NaNs propagated correctly */
	if (std.math.isNaN(x) || std.math.isNaN(alpha)) 
		return x + alpha;
	if (x < 0)
		return double.nan;
	if(alpha < 0)
	alpha = -alpha;
	nb = 1 + cast(int)floor(alpha);/* b.length-1 <= |alpha| < b.length */
	alpha -= nb-1;
	b = cast(double*) calloc(nb, double.sizeof).enforce("besselK allocation error");
	scope(exit) b.free;
	besselK(x, alpha, expFlag, b[0..nb], ncalc);
	if(ncalc != nb) {/* error input */
	  if(ncalc < 0)
		throw new Exception(format("besselK(%g): ncalc (=%ld) != b.length (=%ld); alpha=%g. Arg. out of range?\n",
			 x, ncalc, nb, alpha));
	  else
		throw new Exception(format("besselK(%g,nu=%g): precision lost in result\n",
			 x, alpha+(nb-1)));
	}
	x = b[nb-1];
	return x;
}

///
unittest {
	assert(std.math.approxEqual(besselK(2, 1), 0.139865881816522427284));
	assert(std.math.approxEqual(besselK(20, 1), 5.88305796955703817765028e-10));
	assert(std.math.approxEqual(besselK(1, 1.2), 0.701080));
}

/* modified version of besselK that accepts a work array instead of
	allocating one. */
double besselK(double x, double alpha, Flag!"ExponentiallyScaled" expFlag, double[] b)
{
	ptrdiff_t nb;
	ptrdiff_t ncalc;

	/* NaNs propagated correctly */
	if (std.math.isNaN(x) || std.math.isNaN(alpha)) 
		return x + alpha;
	if (x < 0)
		return double.nan;
	if(alpha < 0)
	alpha = -alpha;
	nb = 1+cast(int)floor(alpha);/* b.length-1 <= |alpha| < b.length */
	alpha -= nb-1;
	besselK(x, alpha, expFlag, b[0..nb], ncalc);
	if(ncalc != nb) {/* error input */
	  if(ncalc < 0)
		throw new Exception(format("besselK(%g): ncalc (=%ld) != b.length (=%ld); alpha=%g. Arg. out of range?\n",
			 x, ncalc, nb, alpha));
	  else
		throw new Exception(format("besselK(%g,nu=%g): precision lost in result\n",
			 x, alpha+(nb-1)));
	}
	x = b[nb-1];
	return x;
}

void besselK(double x, double alpha, Flag!"ExponentiallyScaled" expFlag, double[] b, ref ptrdiff_t ncalc)
{
/*-------------------------------------------------------------------

  This routine calculates modified Bessel functions
  of the third kind, K_(N+ALPHA) (X), for non-negative
  argument X, and non-negative order N+ALPHA, with or without
  exponential scaling.

  Explanation of variables in the calling sequence

 X	 - Non-negative argument for which
	 K's or exponentially scaled K's (K*EXP(X))
	 are to be calculated.	If K's are to be calculated,
	 X must not be greater than XMAX_BESS_K.
 ALPHA - Fractional part of order for which
	 K's or exponentially scaled K's (K*EXP(X)) are
	 to be calculated.  0 <= ALPHA < 1.0.
 NB	- Number of functions to be calculated, NB > 0.
	 The first function calculated is of order ALPHA, and the
	 last is of order (NB - 1 + ALPHA).
 expFlag	- Type.	expFlag = No if unscaled K's are to be calculated,
			= Yes if exponentially scaled K's are to be calculated.
 BK	- Output vector of length NB.	If the
	 routine terminates normally (NCALC=NB), the vector BK
	 contains the functions K(ALPHA,X), ... , K(NB-1+ALPHA,X),
	 or the corresponding exponentially scaled functions.
	 If (0 < NCALC < NB), BK(I) contains correct function
	 values for I <= NCALC, and contains the ratios
	 K(ALPHA+I-1,X)/K(ALPHA+I-2,X) for the rest of the array.
 NCALC - Output variable indicating possible errors.
	 Before using the vector BK, the user should check that
	 NCALC=NB, i.e., all orders have been calculated to
	 the desired accuracy.	See error returns below.


 *******************************************************************

 Error returns

  In case of an error, NCALC != NB, and not all K's are
  calculated to the desired accuracy.

  NCALC < -1:  An argument is out of range. For example,
	NB <= 0, IZE is not 1 or 2, or IZE=1 and ABS(X) >= XMAX_BESS_K.
	In this case, the B-vector is not calculated,
	and NCALC is set to MIN0(NB,0)-2	 so that NCALC != NB.
  NCALC = -1:  Either  K(ALPHA,X) >= XINF  or
	K(ALPHA+NB-1,X)/K(ALPHA+NB-2,X) >= XINF.	 In this case,
	the B-vector is not calculated.	Note that again
	NCALC != NB.

  0 < NCALC < NB: Not all requested function values could
	be calculated accurately.  BK(I) contains correct function
	values for I <= NCALC, and contains the ratios
	K(ALPHA+I-1,X)/K(ALPHA+I-2,X) for the rest of the array.


 Intrinsic functions required are:

	 ABS, AINT, EXP, INT, LOG, MAX, MIN, SINH, SQRT


 Acknowledgement

	This program is based on a program written b J. B. Campbell
	(2) that computes values of the Bessel functions K of float
	argument and float order.  Modifications include the addition
	of non-scaled functions, parameterization of machine
	dependencies, and the use of more accurate approximations
	for SINH and SIN.

 References: "On Temme's Algorithm for the Modified Bessel
		  Functions of the Third Kind," Campbell, J. B.,
		  TOMS 6(4), Dec. 1980, pp. 581-586.

		 "A FORTRAN IV Suaoutine for the Modified Bessel
		  Functions of the Third Kind of Real Order and Real
		  Argument," Campbell, J. B., Report NRC/ERB-925,
		  National Research Council, Canada.

  Latest modification: May 30, 1989

  Modified b: W. J. Cody and L. Stoltz
			Applied Mathematics Division
			Argonne National Laboratory
			Argonne, IL  60439

 -------------------------------------------------------------------
*/
	/*---------------------------------------------------------------------
	 * Mathematical constants
	 *	A = LOG(2) - Euler's constant
	 *	D = SQRT(2/PI)
	 ---------------------------------------------------------------------*/
	static immutable double a = .11593151565841244881;

	/*---------------------------------------------------------------------
	  P, Q - Approximation for LOG(GAMMA(1+ALPHA))/ALPHA + Euler's constant
	  Coefficients converted from hex to decimal and modified
	  b W. J. Cody, 2/26/82 */
	static immutable double[8] p = [ .805629875690432845,20.4045500205365151,
		157.705605106676174,536.671116469207504,900.382759291288778,
		730.923886650660393,229.299301509425145,.822467033424113231 ];
	static immutable double[7] q = [ 29.4601986247850434,277.577868510221208,
		1206.70325591027438,2762.91444159791519,3443.74050506564618,
		2210.63190113378647,572.267338359892221 ];
	/* R, S - Approximation for (1-ALPHA*PI/SIN(ALPHA*PI))/(2.D0*ALPHA) */
	static immutable double[5] r = [ -.48672575865218401848,13.079485869097804016,
		-101.96490580880537526,347.65409106507813131,
		3.495898124521934782e-4 ];
	static immutable double[4] s = [ -25.579105509976461286,212.57260432226544008,
		-610.69018684944109624,422.69668805777760407 ];
	/* T	- Approximation for SINH(Y)/Y */
	static immutable double[6] t = [ 1.6125990452916363814e-10,
		2.5051878502858255354e-8,2.7557319615147964774e-6,
		1.9841269840928373686e-4,.0083333333333334751799,
		.16666666666666666446 ];
	/*---------------------------------------------------------------------*/
	static immutable double[6] estm = [ 52.0583,5.7607,2.7782,14.4303,185.3004, 9.3715 ];
	static immutable double[7] estf = [ 41.8341,7.1075,6.4306,42.511,1.35633,84.5096,20.];

	/* Local variables */
	ptrdiff_t iend, i, j, k, m, ii, mplus1;
	double x2b4, twox, c, blpha, ratio, wminf;
	double d1, d2, d3, f0, f1, f2, p0, q0, t1, t2, twonu;
	double dm, ex, b1, b2, nu;

	ii = 0; /* -Wall */

	ex = x;
	nu = alpha;
	ncalc = -2;
	if (b.length > 0 && (0. <= nu && nu < 1.)) {
	if(ex <= 0 || (expFlag == false && ex > xmax_BESS_K)) {
		if(ex <= 0) {
			enforce(ex == 0, "besselK Range Error");
		for(i=0; i < b.length; i++)
			b[i] = double.infinity;
		} else /* would only have underflow */
		for(i=0; i < b.length; i++)
			b[i] = 0;
		ncalc = b.length;
		return;
	}
	k = 0;
	if (nu < sqxmin_BESS_K) {
		nu = 0;
	} else if (nu > .5) {
		k = 1;
		nu -= 1.;
	}
	twonu = nu + nu;
	iend = b.length + k - 1;
	c = nu * nu;
	d3 = -c;
	if (ex <= 1.) {
		/* ------------------------------------------------------------
			Calculation of P0 = GAMMA(1+ALPHA) * (2/X)**ALPHA
				  Q0 = GAMMA(1-ALPHA) * (X/2)**ALPHA
			------------------------------------------------------------ */
		d1 = 0; d2 = p[0];
		t1 = 1.; t2 = q[0];
		for (i = 2; i <= 7; i += 2) {
		d1 = c * d1 + p[i - 1];
		d2 = c * d2 + p[i];
		t1 = c * t1 + q[i - 1];
		t2 = c * t2 + q[i];
		}
		d1 = nu * d1;
		t1 = nu * t1;
		f1 = log(ex);
		f0 = a + nu * (p[7] - nu * (d1 + d2) / (t1 + t2)) - f1;
		q0 = exp(-nu * (a - nu * (p[7] + nu * (d1-d2) / (t1-t2)) - f1));
		f1 = nu * f0;
		p0 = exp(f1);
		/* -----------------------------------------------------------
			Calculation of F0 =
			----------------------------------------------------------- */
		d1 = r[4];
		t1 = 1.;
		for (i = 0; i < 4; ++i) {
		d1 = c * d1 + r[i];
		t1 = c * t1 + s[i];
		}
		/* d2 := sinh(f1)/ nu = sinh(f1)/(f1/f0)
		 *		= f0 * sinh(f1)/f1 */
		if (fabs(f1) <= .5) {
		f1 *= f1;
		d2 = 0;
		for (i = 0; i < 6; ++i) {
			d2 = f1 * d2 + t[i];
		}
		d2 = f0 + f0 * f1 * d2;
		} else {
		d2 = sinh(f1) / nu;
		}
		f0 = d2 - nu * d1 / (t1 * p0);
		if (ex <= 1e-10) {
		/* ---------------------------------------------------------
			X <= 1.0E-10
			Calculation of K(ALPHA,X) and X*K(ALPHA+1,X)/K(ALPHA,X)
			--------------------------------------------------------- */
		b[0] = f0 + ex * f0;
		if (expFlag == false) {
			b[0] -= ex * b[0];
		}
		ratio = p0 / f0;
		c = ex * double.max;
		if (k != 0) {
			/* ---------------------------------------------------
				Calculation of K(ALPHA,X)
				and  X*K(ALPHA+1,X)/K(ALPHA,X),	ALPHA >= 1/2
				--------------------------------------------------- */
			ncalc = -1;
			if (b[0] >= c / ratio) {
			return;
			}
			b[0] = ratio * b[0] / ex;
			twonu += 2.;
			ratio = twonu;
		}
		ncalc = 1;
		if (b.length == 1)
			return;

		/* -----------------------------------------------------
			Calculate  K(ALPHA+L,X)/K(ALPHA+L-1,X),
			L = 1, 2, ... , NB-1
			----------------------------------------------------- */
		ncalc = -1;
		for (i = 1; i < b.length; ++i) {
			if (ratio >= c)
			return;

			b[i] = ratio / ex;
			twonu += 2.;
			ratio = twonu;
		}
		ncalc = 1;
		goto L420;
		} else {
		/* ------------------------------------------------------
			10^-10 < X <= 1.0
			------------------------------------------------------ */
		c = 1.;
		x2b4 = ex * ex / 4.;
		p0 = .5 * p0;
		q0 = .5 * q0;
		d1 = -1.;
		d2 = 0;
		b1 = 0;
		b2 = 0;
		f1 = f0;
		f2 = p0;
		do {
			d1 += 2.;
			d2 += 1.;
			d3 = d1 + d3;
			c = x2b4 * c / d2;
			f0 = (d2 * f0 + p0 + q0) / d3;
			p0 /= d2 - nu;
			q0 /= d2 + nu;
			t1 = c * f0;
			t2 = c * (p0 - d2 * f0);
			b1 += t1;
			b2 += t2;
		} while (fabs(t1 / (f1 + b1)) > double.epsilon ||
			 fabs(t2 / (f2 + b2)) > double.epsilon);
		b1 = f1 + b1;
		b2 = 2. * (f2 + b2) / ex;
		if (expFlag) {
			d1 = exp(ex);
			b1 *= d1;
			b2 *= d1;
		}
		wminf = estf[0] * ex + estf[1];
		}
	} else if (double.epsilon * ex > 1.) {
		/* -------------------------------------------------
			X > 1./EPS
			------------------------------------------------- */
		ncalc = b.length;
		b1 = 1. / (M_SQRT_2dPI * sqrt(ex));
		for (i = 0; i < b.length; ++i)
		b[i] = b1;
		return;

	} else {
		/* -------------------------------------------------------
			X > 1.0
			------------------------------------------------------- */
		twox = ex + ex;
		blpha = 0;
		ratio = 0;
		if (ex <= 4.) {
		/* ----------------------------------------------------------
			Calculation of K(ALPHA+1,X)/K(ALPHA,X),  1.0 <= X <= 4.0
			----------------------------------------------------------*/
		d2 = trunc(estm[0] / ex + estm[1]);
		m = cast(int) d2;
		d1 = d2 + d2;
		d2 -= .5;
		d2 *= d2;
		for (i = 2; i <= m; ++i) {
			d1 -= 2.;
			d2 -= d1;
			ratio = (d3 + d2) / (twox + d1 - ratio);
		}
		/* -----------------------------------------------------------
			Calculation of I(|ALPHA|,X) and I(|ALPHA|+1,X) by backward
			recurrence and K(ALPHA,X) from the wronskian
			-----------------------------------------------------------*/
		d2 = trunc(estm[2] * ex + estm[3]);
		m = cast(int) d2;
		c = fabs(nu);
		d3 = c + c;
		d1 = d3 - 1.;
		f1 = double.min_normal;
		f0 = (2. * (c + d2) / ex + .5 * ex / (c + d2 + 1.)) * double.min_normal;
		for (i = 3; i <= m; ++i) {
			d2 -= 1.;
			f2 = (d3 + d2 + d2) * f0;
			blpha = (1. + d1 / d2) * (f2 + blpha);
			f2 = f2 / ex + f1;
			f1 = f0;
			f0 = f2;
		}
		f1 = (d3 + 2.) * f0 / ex + f1;
		d1 = 0;
		t1 = 1.;
		for (i = 1; i <= 7; ++i) {
			d1 = c * d1 + p[i - 1];
			t1 = c * t1 + q[i - 1];
		}
		p0 = exp(c * (a + c * (p[7] - c * d1 / t1) - log(ex))) / ex;
		f2 = (c + .5 - ratio) * f1 / ex;
		b1 = p0 + (d3 * f0 - f2 + f0 + blpha) / (f2 + f1 + f0) * p0;
		if (expFlag == false) {
			b1 *= exp(-ex);
		}
		wminf = estf[2] * ex + estf[3];
		} else {
		/* ---------------------------------------------------------
			Calculation of K(ALPHA,X) and K(ALPHA+1,X)/K(ALPHA,X), by
			backward recurrence, for  X > 4.0
			----------------------------------------------------------*/
		dm = trunc(estm[4] / ex + estm[5]);
		m = cast(int) dm;
		d2 = dm - .5;
		d2 *= d2;
		d1 = dm + dm;
		for (i = 2; i <= m; ++i) {
			dm -= 1.;
			d1 -= 2.;
			d2 -= d1;
			ratio = (d3 + d2) / (twox + d1 - ratio);
			blpha = (ratio + ratio * blpha) / dm;
		}
		b1 = 1. / ((M_SQRT_2dPI + M_SQRT_2dPI * blpha) * sqrt(ex));
		if (expFlag == false)
			b1 *= exp(-ex);
		wminf = estf[4] * (ex - fabs(ex - estf[6])) + estf[5];
		}
		/* ---------------------------------------------------------
			Calculation of K(ALPHA+1,X)
			from K(ALPHA,X) and  K(ALPHA+1,X)/K(ALPHA,X)
			--------------------------------------------------------- */
		b2 = b1 + b1 * (nu + .5 - ratio) / ex;
	}
	/*--------------------------------------------------------------------
	  Calculation of 'NCALC', K(ALPHA+I,X),	I  =  0, 1, ... , NCALC-1,
	  &	  K(ALPHA+I,X)/K(ALPHA+I-1,X),	I = NCALC, NCALC+1, ... , NB-1
	  -------------------------------------------------------------------*/
	ncalc = b.length;
	b[0] = b1;
	if (iend == 0)
		return;

	j = 1 - k;
	if (j >= 0)
		b[j] = b2;

	if (iend == 1)
		return;

	m = min(cast(int) (wminf - nu),iend);
	for (i = 2; i <= m; ++i) {
		t1 = b1;
		b1 = b2;
		twonu += 2.;
		if (ex < 1.) {
		if (b1 >= double.max / twonu * ex)
			break;
		} else {
		if (b1 / ex >= double.max / twonu)
			break;
		}
		b2 = twonu / ex * b1 + t1;
		ii = i;
		++j;
		if (j >= 0) {
		b[j] = b2;
		}
	}

	m = ii;
	if (m == iend) {
		return;
	}
	ratio = b2 / b1;
	mplus1 = m + 1;
	ncalc = -1;
	for (i = mplus1; i <= iend; ++i) {
		twonu += 2.;
		ratio = twonu / ex + 1./ratio;
		++j;
		if (j >= 1) {
		b[j] = ratio;
		} else {
		if (b2 >= double.max / ratio)
			return;

		b2 *= ratio;
		}
	}
	ncalc = max(1, mplus1 - k);
	if (ncalc == 1)
		b[0] = b2;
	if (b.length == 1)
		return;

L420:
	for (i = ncalc; i < b.length; ++i) { /* i == ncalc */
		if (b[i-1] >= double.max / b[i])
			return;
		b[i] *= b[i-1];
		ncalc++;
	}
	}
}


///
double besselI(double x, double alpha, Flag!"ExponentiallyScaled" expFlag = Flag!"ExponentiallyScaled".no)
{
	ptrdiff_t nb;
	ptrdiff_t ncalc;
	double na;
	double* b;

	/* NaNs propagated correctly */
	if (std.math.isNaN(x) || std.math.isNaN(alpha)) 
		return x + alpha;
	if (x < 0)
		return double.nan;
	na = floor(alpha);
	if (alpha < 0) {
	/* Using Aaamowitz & Stegun  9.6.2 & 9.6.6
	 * this may not be quite optimal (CPU and accuracy wise) */
	return(besselI(x, -alpha, expFlag) +
			((alpha == na) ? /* sin(pi * alpha) = 0 */ 0 :
		besselK(x, -alpha, expFlag) *
		(expFlag == true ? 2. : 2.*exp(-2.0 * x)) / cast(double) std.math.PI * sinPi(-alpha)));
	}
	nb = 1 + cast(int)na;/* b.length-1 <= alpha < b.length */
	alpha -= nb-1;
	b = cast(double*) calloc(nb, double.sizeof).enforce("besselI allocation error");
	scope(exit) b.free;
	besselI(x, alpha, expFlag, b[0..nb], ncalc);
	if(ncalc != nb) {/* error input */
	if(ncalc < 0)
			throw new Exception(format("besselI(%g): ncalc (=%ld) != b.length (=%ld); alpha=%g. Arg. out of range?\n",
				 x, ncalc, nb, alpha));
	else
			throw new Exception(format("besselI(%g,nu=%g): precision lost in result\n",
				 x, alpha+(nb-1)));
	}
	x = b[nb-1];
	return x;
}

///
unittest {
	assert(std.math.approxEqual(besselI(2, 1), 1.590636854637329063382254424999666));
	assert(std.math.approxEqual(besselI(20, 1), 4.245497338512777018140990665e+7));
	assert(std.math.approxEqual(besselI(1, 1.2), 0.441739));
}


/* modified version of besselI that accepts a work array instead of
	allocating one. */
double besselI(double x, double alpha, Flag!"ExponentiallyScaled" expFlag, double[] b)
{
	ptrdiff_t nb;
	ptrdiff_t ncalc;
	double na;

	/* NaNs propagated correctly */
	if (std.math.isNaN(x) || std.math.isNaN(alpha)) 
		return x + alpha;
	if (x < 0)
		return double.nan;
	na = floor(alpha);
	if (alpha < 0) {
	/* Using Aaamowitz & Stegun  9.6.2 & 9.6.6
	 * this may not be quite optimal (CPU and accuracy wise) */
	return(besselI(x, -alpha, expFlag, b) +
			((alpha == na) ? 0 :
		besselK(x, -alpha, expFlag, b) *
		(expFlag == true ? 2. : 2.*exp(-2.0 * x))/cast(double)std.math.PI * sinPi(-alpha)));
	}
	nb = 1 + cast(int)na;/* b.length-1 <= alpha < b.length */
	alpha -= nb-1;
	besselI(x, alpha, expFlag, b[0..nb], ncalc);
	if(ncalc != nb) {/* error input */
	if(ncalc < 0)
			throw new Exception(format("besselI(%g): ncalc (=%ld) != b.length (=%ld); alpha=%g. Arg. out of range?\n",
				 x, ncalc, nb, alpha));
	else
			throw new Exception(format("besselI(%g,nu=%g): precision lost in result\n",
				 x, alpha+(nb-1)));
	}
	x = b[nb-1];
	return x;
}

void besselI(double x, double alpha, Flag!"ExponentiallyScaled" expFlag, double[] b, ref ptrdiff_t ncalc)
{
/* -------------------------------------------------------------------

 This routine calculates Bessel functions I_(N+ALPHA) (X)
 for non-negative argument X, and non-negative order N+ALPHA,
 with or without exponential scaling.


 Explanation of variables in the calling sequence

 X	 - Non-negative argument for which
	 I's or exponentially scaled I's (I*EXP(-X))
	 are to be calculated.	If I's are to be calculated,
	 X must be less than exparg_BESS (IZE=1) or xlrg_BESS_IJ (IZE=2),
	 (see bessel.h).
 ALPHA - Fractional part of order for which
	 I's or exponentially scaled I's (I*EXP(-X)) are
	 to be calculated.  0 <= ALPHA < 1.0.
 NB	- Number of functions to be calculated, NB > 0.
	 The first function calculated is of order ALPHA, and the
	 last is of order (NB - 1 + ALPHA).
 IZE	- Type.	IZE = 1 if unscaled I's are to be calculated,
			= 2 if exponentially scaled I's are to be calculated.
 BI	- Output vector of length NB.	If the routine
	 terminates normally (NCALC=NB), the vector BI contains the
	 functions I(ALPHA,X) through I(NB-1+ALPHA,X), or the
	 corresponding exponentially scaled functions.
 NCALC - Output variable indicating possible errors.
	 Before using the vector BI, the user should check that
	 NCALC=NB, i.e., all orders have been calculated to
	 the desired accuracy.	See error returns below.


 *******************************************************************
 *******************************************************************

 Error returns

  In case of an error,	NCALC != NB, and not all I's are
  calculated to the desired accuracy.

  NCALC < 0:  An argument is out of range. For example,
	 NB <= 0, IZE is not 1 or 2, or IZE=1 and ABS(X) >= EXPARG_BESS.
	 In this case, the BI-vector is not calculated, and NCALC is
	 set to MIN0(NB,0)-1 so that NCALC != NB.

  NB > NCALC > 0: Not all requested function values could
	 be calculated accurately.	This usually occurs because NB is
	 much larger than ABS(X).  In this case, BI[N] is calculated
	 to the desired accuracy for N <= NCALC, but precision
	 is lost for NCALC < N <= NB.  If BI[N] does not vanish
	 for N > NCALC (because it is too small to be represented),
	 and BI[N]/BI[NCALC] = 10**(-K), then only the first NSIG-K
	 significant figures of BI[N] can be trusted.


 Intrinsic functions required are:

	 DBLE, EXP, gamma_cody, INT, MAX, MIN, REAL, SQRT


 Acknowledgement

  This program is based on a program written b David J.
  Sookne (2) that computes values of the Bessel functions J or
  I of float argument and long order.  Modifications include
  the restriction of the computation to the I Bessel function
  of non-negative float argument, the extension of the computation
  to arbtrary positive order, the inclusion of optional
  exponential scaling, and the elimination of most underflow.
  An earlier version was published in (3).

 References: "A Note on Backward Recurrence Algorithms," Olver,
		  F. W. J., and Sookne, D. J., Math. Comp. 26, 1972,
		  pp 941-947.

		 "Bessel Functions of Real Argument and Integer Order,"
		  Sookne, D. J., NBS Jour. of Res. B. 77B, 1973, pp
		  125-132.

		 "ALGORITHM 597, Sequence of Modified Bessel Functions
		  of the First Kind," Cody, W. J., Trans. Math. Soft.,
		  1983, pp. 242-245.

  Latest modification: May 30, 1989

  Modified b: W. J. Cody and L. Stoltz
			Applied Mathematics Division
			Argonne National Laboratory
			Argonne, IL  60439
*/

	/*-------------------------------------------------------------------
	  Mathematical constants
	  -------------------------------------------------------------------*/
	enum double const__ = 1.585;

	/* Local variables */
	ptrdiff_t nend, intx, nbmx, k, l, n, nstart;
	double pold, test,	p, em, en, empal, emp2al, halfx,
	aa, bb, cc, psave, plast, tover, psavel, sum, nu, twonu;

	/*Parameter adjustments */
//	--b;
	nu = alpha;
	twonu = nu + nu;

	/*-------------------------------------------------------------------
	  Check for X, NB, OR IZE out of range.
	  ------------------------------------------------------------------- */
	if (b.length > 0 && x >= 0. &&	(0. <= nu && nu < 1.)  ) {

	ncalc = b.length;
	if(expFlag == false && x > exparg_BESS) {
		b[] = double.infinity; /* the limit *is* = Inf */
		return;
	}
	if(expFlag && x > xlrg_BESS_IJ) {
		b[] = 0; /* The limit exp(-x) * I_nu(x) --> 0 : */
		return;
	}
	intx = cast(int) (x);/* fine, since x <= xlrg_BESS_IJ <<< LONG_MAX */
	if (x >= rtnsig_BESS) { /* "non-small" x ( >= 1e-4 ) */
/* -------------------------------------------------------------------
	Initialize the forward sweep, the P-sequence of Olver
	------------------------------------------------------------------- */
		nbmx = b.length - intx;
		n = intx + 1;
		en = cast(double) (n + n) + twonu;
		plast = 1.;
		p = en / x;
		/* ------------------------------------------------
			Calculate general significance test
			------------------------------------------------ */
		test = ensig_BESS + ensig_BESS;
		if (intx << 1 > nsig_BESS * 5) {
		test = sqrt(test * p);
		} else {
		test /= const__ ^^ intx;
		}
		if (nbmx >= 3) {
		/* --------------------------------------------------
			Calculate P-sequence until N = NB-1
			Check for possible overflow.
			------------------------------------------------ */
		tover = enten_BESS / ensig_BESS;
		nstart = intx + 2;
		nend = b.length - 1;
		for (k = nstart; k <= nend; ++k) {
			n = k;
			en += 2.;
			pold = plast;
			plast = p;
			p = en * plast / x + pold;
			if (p > tover) {
			/* ------------------------------------------------
				To avoid overflow, divide P-sequence b TOVER.
				Calculate P-sequence until ABS(P) > 1.
				---------------------------------------------- */
			tover = enten_BESS;
			p /= tover;
			plast /= tover;
			psave = p;
			psavel = plast;
			nstart = n + 1;
			do {
				++n;
				en += 2.;
				pold = plast;
				plast = p;
				p = en * plast / x + pold;
			}
			while (p <= 1.);

			bb = en / x;
			/* ------------------------------------------------
				Calculate backward test, and find NCALC,
				the highest N such that the test is passed.
				------------------------------------------------ */
			test = pold * plast / ensig_BESS;
			test *= .5 - .5 / (bb * bb);
			p = plast * tover;
			--n;
			en -= 2.;
			nend = min(b.length,n);
			for (l = nstart; l <= nend; ++l) {
				ncalc = l;
				pold = psavel;
				psavel = psave;
				psave = en * psavel / x + pold;
				if (psave * psavel > test) {
				goto L90;
				}
			}
			ncalc = nend + 1;
L90:
			ncalc--;
			goto L120;
			}
		}
		n = nend;
		en = cast(double)(n + n) + twonu;
		/*---------------------------------------------------
		  Calculate special significance test for NBMX > 2.
		  --------------------------------------------------- */
		test = max(test, sqrt(plast * ensig_BESS) * sqrt(p + p));
		}
		/* --------------------------------------------------------
			Calculate P-sequence until significance test passed.
			-------------------------------------------------------- */
		do {
		++n;
		en += 2.;
		pold = plast;
		plast = p;
		p = en * plast / x + pold;
		} while (p < test);

L120:
/* -------------------------------------------------------------------
 Initialize the backward recursion and the normalization sum.
 ------------------------------------------------------------------- */
		++n;
		en += 2.;
		bb = 0;
		aa = 1. / p;
		em = cast(double) n - 1.;
		empal = em + nu;
		emp2al = em - 1. + twonu;
		sum = aa * empal * emp2al / em;
		nend = n - b.length;
		if (nend < 0) {
		/* -----------------------------------------------------
			N < NB, so store BI[N] and set higher orders to 0..
			----------------------------------------------------- */
		b[n - 1] = aa;
		nend = -nend;
		b[n .. n + nend] = 0;
		} else {
		if (nend > 0) {
			/* -----------------------------------------------------
				Recur backward via difference equation,
				calculating (but not storing) BI[N], until N = NB.
				--------------------------------------------------- */

			for (l = 1; l <= nend; ++l) {
			--n;
			en -= 2.;
			cc = bb;
			bb = aa;
			/* for x ~= 1500,  sum would overflow to 'inf' here,
			 * and the final b[] /= sum would give 0 wrongly;
			 * RE-normalize (aa, sum) here -- no need to undo */
			if(nend > 100 && aa > 1e200) {
				/* multiply b  2^-900 = 1.18e-271 */
				cc	= ldexp(cc, -900);
				bb	= ldexp(bb, -900);
				sum = ldexp(sum,-900);
			}
			aa = en * bb / x + cc;
			em -= 1.;
			emp2al -= 1.;
			if (n == 1) {
				break;
			}
			if (n == 2) {
				emp2al = 1.;
			}
			empal -= 1.;
			sum = (sum + aa * empal) * emp2al / em;
			}
		}
		/* ---------------------------------------------------
			Store BI[NB]
			--------------------------------------------------- */
		b[n - 1] = aa;
		if (b.length <= 1) {
			sum = sum + sum + aa;
			goto L230;
		}
		/* -------------------------------------------------
			Calculate and Store BI[NB-1]
			------------------------------------------------- */
		--n;
		en -= 2.;
		b[n - 1] = en * aa / x + bb;
		if (n == 1) {
			goto L220;
		}
		em -= 1.;
		if (n == 2)
			emp2al = 1.;
		else
			emp2al -= 1.;

		empal -= 1.;
		sum = (sum + b[n - 1] * empal) * emp2al / em;
		}
		nend = n - 2;
		if (nend > 0) {
			/* --------------------------------------------
				Calculate via difference equation
				and store BI[N], until N = 2.
				------------------------------------------ */
			for (l = 1; l <= nend; ++l) {
				--n;
				en -= 2.;
				b[n - 1] = en * b[n + 0] / x + b[n + 1];
				em -= 1.;
				if (n == 2)
					emp2al = 1.;
				else
					emp2al -= 1.;
				empal -= 1.;
				sum = (sum + b[n - 1] * empal) * emp2al / em;
			}
		}
		/* ----------------------------------------------
			Calculate BI[1]
			-------------------------------------------- */
		b[0] = 2. * empal * b[1] / x + b[2];
L220:
		sum = sum + sum + b[0];

L230:
		/* ---------------------------------------------------------
			Normalize.  Divide all BI[N] b sum.
			--------------------------------------------------------- */
		if (nu != 0.)
			sum *= (gamma_cody(1. + nu) * pow(x * .5, -nu));
		if (expFlag == false)
			sum *= exp(-(x));
		aa = enmten_BESS;
		if (sum > 1.)
		aa *= sum;
		for (n = 1; n <= b.length; ++n) {
			if (b[n - 1] < aa)
				b[n - 1] = 0;
			else
				b[n - 1] /= sum;
		}
		return;
	} else { /* small x  < 1e-4 */
		/* -----------------------------------------------------------
			Two-term ascending series for small X.
			-----------------------------------------------------------*/
		aa = 1.;
		empal = 1. + nu;
		/* No need to check for underflow */
		halfx = .5 * x;
		if (nu != 0.)
		aa = pow(halfx, nu) / gamma_cody(empal);
		if (expFlag)
		aa *= exp(-(x));
		bb = halfx * halfx;
		b[0] = aa + aa * bb / empal;
		if (x != 0. && b[0] == 0.)
		ncalc = 0;
		if (b.length > 1) {
		if (x == 0.) {
			b[1..$] = 0;
		} else {
			/* -------------------------------------------------
				Calculate higher-order functions.
				------------------------------------------------- */
			cc = halfx;
			tover = (enmten_BESS + enmten_BESS) / x;
			if (bb != 0.)
			tover = enmten_BESS / bb;
			for (n = 2; n <= b.length; ++n) {
				aa /= empal;
				empal += 1.;
				aa *= cc;
				if (aa <= tover * empal)
					b[n - 1] = aa = 0;
				else
					b[n - 1] = aa + aa * bb / empal;
				if (b[n - 1] == 0. && ncalc > n)
					ncalc = n - 1;
			}
		}
		}
	}
	} else { /* argument out of range */
		ncalc = min(b.length,0) - 1;
	}
}

///
double besselJ(double x, double alpha)
{
	ptrdiff_t nb, ncalc;
	double na;
	double* b;

	/* NaNs propagated correctly */
	if (std.math.isNaN(x) || std.math.isNaN(alpha)) 
		return x + alpha;
	if (x < 0)
		return double.nan;
	na = floor(alpha);
	if (alpha < 0) {
	/* Using Aaamowitz & Stegun  9.1.2
	 * this may not be quite optimal (CPU and accuracy wise) */
	return(besselJ(x, -alpha) * cosPi(alpha) +
			((alpha == na) ? 0 :
			besselY(x, -alpha) * sinPi(alpha)));
	}
	nb = 1 + cast(int)na; /* b.length-1 <= alpha < b.length */
	alpha -= nb-1;
	b = cast(double*) calloc(nb, double.sizeof).enforce("besselJ allocation error");
	scope(exit) b.free;
	besselJ(x, alpha, b[0..nb], ncalc);
	if(ncalc != nb) {/* error input */
		if(ncalc < 0)
			throw new Exception(format("besselJ(%g): ncalc (=%ld) != b.length (=%ld); alpha=%g. Arg. out of range?\n",
				x, ncalc, nb, alpha));
	 	else
			throw new Exception(format("besselJ(%g,nu=%g): precision lost in result\n",
				x, alpha+(nb-1)));
	}
	x = b[nb-1];
	return x;
}

///
unittest {
	assert(std.math.approxEqual(besselJ(2, 1), 0.5767248077568733872024482422691));
	assert(std.math.approxEqual(besselJ(20, 1), 0.066833124175850045578992974193));
	assert(std.math.approxEqual(besselJ(1, 1.2), 0.351884));
}

/* modified version of besselJ that accepts a work array instead of
	allocating one. */
double besselJ(double x, double alpha, double[] b)
{
	ptrdiff_t nb, ncalc;
	double na;

	/* NaNs propagated correctly */
	if (std.math.isNaN(x) || std.math.isNaN(alpha)) 
		return x + alpha;
	if (x < 0)
		return double.nan;
	na = floor(alpha);
	if (alpha < 0) {
	/* Using Aaamowitz & Stegun  9.1.2
	 * this may not be quite optimal (CPU and accuracy wise) */
	return(besselJ(x, -alpha, b) * cosPi(alpha) +
			((alpha == na) ? 0 :
		besselY(x, -alpha, b) * sinPi(alpha)));
	}
	nb = 1 + cast(int)na; /* b.length-1 <= alpha < b.length */
	alpha -= nb-1;
	besselJ(x, alpha, b[0..nb], ncalc);
	if(ncalc != nb) {/* error input */
	  if(ncalc < 0)
		throw new Exception(format("besselJ(%g): ncalc (=%ld) != b.length (=%ld); alpha=%g. Arg. out of range?\n",
			 x, ncalc, nb, alpha));
	  else
		throw new Exception(format("besselJ(%g,nu=%g): precision lost in result\n",
			 x, alpha+(nb-1)));
	}
	x = b[b.length-1];
	return x;
}

void besselJ(double x, double alpha, double[] b, ref ptrdiff_t ncalc)
{
/*
 Calculates Bessel functions J_{n+alpha} (x)
 for non-negative argument x, and non-negative order n+alpha, n = 0,1,..,b.length-1.

  Explanation of variables in the calling sequence.

 X	 - Non-negative argument for which J's are to be calculated.
 ALPHA - Fractional part of order for which
	 J's are to be calculated.  0 <= ALPHA < 1.
 NB	- Number of functions to be calculated, NB >= 1.
	 The first function calculated is of order ALPHA, and the
	 last is of order (NB - 1 + ALPHA).
 B	 - Output vector of length NB.  If RJBESL
	 terminates normally (NCALC=NB), the vector B contains the
	 functions J/ALPHA/(X) through J/NB-1+ALPHA/(X).
 NCALC - Output variable indicating possible errors.
	 Before using the vector B, the user should check that
	 NCALC=NB, i.e., all orders have been calculated to
	 the desired accuracy.	See the following

	 ****************************************************************

 Error return codes

	In case of an error,  NCALC != NB, and not all J's are
	calculated to the desired accuracy.

	NCALC < 0:	An argument is out of range. For example,
		NBES <= 0, ALPHA < 0 or > 1, or X is too large.
		In this case, b[1] is set to zero, the remainder of the
		B-vector is not calculated, and NCALC is set to
		MIN(NB,0)-1 so that NCALC != NB.

	NB > NCALC > 0: Not all requested function values could
		be calculated accurately.  This usually occurs because NB is
		much larger than ABS(X).	 In this case, b[N] is calculated
		to the desired accuracy for N <= NCALC, but precision
		is lost for NCALC < N <= NB.  If b[N] does not vanish
		for N > NCALC (because it is too small to be represented),
		and b[N]/b[NCALC] = 10^(-K), then only the first NSIG - K
		significant figures of b[N] can be trusted.


  Acknowledgement

	This program is based on a program written b David J. Sookne
	(2) that computes values of the Bessel functions J or I of float
	argument and long order.  Modifications include the restriction
	of the computation to the J Bessel function of non-negative float
	argument, the extension of the computation to arbtrary positive
	order, and the elimination of most underflow.

  References:

	Olver, F.W.J., and Sookne, D.J. (1972)
	"A Note on Backward Recurrence Algorithms";
	Math. Comp. 26, 941-947.

	Sookne, D.J. (1973)
	"Bessel Functions of Real Argument and Integer Order";
	NBS Jour. of Res. B. 77B, 125-132.

  Latest modification: March 19, 1990

  Author: W. J. Cody
	  Applied Mathematics Division
	  Argonne National Laboratory
	  Argonne, IL  60439
 *******************************************************************
 */

/* ---------------------------------------------------------------------
  Mathematical constants

	PI2	  = 2 / PI
	TWOPI1 = first few significant digits of 2 * PI
	TWOPI2 = (2*PI - TWOPI1) to working precision, i.e.,
		TWOPI1 + TWOPI2 = 2 * PI to extra precision.
 --------------------------------------------------------------------- */
	static immutable double pi2 = .636619772367581343075535;
	static immutable double twopi1 = 6.28125;
	static immutable double twopi2 =  .001935307179586476925286767;

/*---------------------------------------------------------------------
 *  Factorial(N)
 *--------------------------------------------------------------------- */
	static immutable double[25] fact = [ 1.,1.,2.,6.,24.,120.,720.,5040.,40320.,
		362880.,3628800.,39916800.,479001600.,6227020800.,87178291200.,
		1.307674368e12,2.0922789888e13,3.55687428096e14,6.402373705728e15,
		1.21645100408832e17,2.43290200817664e18,5.109094217170944e19,
		1.12400072777760768e21,2.585201673888497664e22,
		6.2044840173323943936e23 ];

	/* Local variables */
	ptrdiff_t nend, intx, nbmx, i, j, k, l, m, n, nstart;

	double nu, twonu, capp, capq, pold, vcos, test, vsin;
	double p, s, t, z, alpem, halfx, aa, bb, cc, psave, plast;
	double tover, t1, alp2em, em, en, xc, xk, xm, psavel, gnu, xin, sum;


	/* Parameter adjustment */
//	--b;

	nu = alpha;
	twonu = nu + nu;

	/*-------------------------------------------------------------------
	  Check for out of range arguments.
	  -------------------------------------------------------------------*/
	if (b.length > 0 && x >= 0. && 0. <= nu && nu < 1.) {

	ncalc = b.length;
	if(x > xlrg_BESS_IJ) {
		/* indeed, the limit is 0,
		 * but the cutoff happens too early */
		b[] = double.infinity;
		return;
	}
	intx = cast(int) (x);
	/* Initialize result array to zero. */
	b[] = 0;

	/*===================================================================
	  Branch into  3 cases :
	  1) use 2-term ascending series for small X
	  2) use asymptotic form for large X when NB is not too large
	  3) use recursion otherwise
	  ===================================================================*/

	if (x < rtnsig_BESS) {
	  /* ---------------------------------------------------------------
		 Two-term ascending series for small X.
		 --------------------------------------------------------------- */
		alpem = 1. + nu;

		halfx = (x > enmten_BESS) ? .5 * x :  0;
		aa	  = (nu != 0.)	  ? pow(halfx, nu) / (nu * gamma_cody(nu)) : 1.;
		bb	  = (x + 1. > 1.)? -halfx * halfx : 0;
		b[0] = aa + aa * bb / alpem;
		if (x != 0. && b[0] == 0.)
		ncalc = 0;

		if (b.length != 1) {
		if (x <= 0.) {
			for (n = 2; n <= b.length; ++n)
			b[n - 1] = 0;
		}
		else {
			/* ----------------------------------------------
				Calculate higher order functions.
				---------------------------------------------- */
			if (bb == 0.)
				tover = (enmten_BESS + enmten_BESS) / x;
			else
				tover = enmten_BESS / bb;
			cc = halfx;
			for (n = 2; n <= b.length; ++n) {
				aa /= alpem;
				alpem += 1.;
				aa *= cc;
				if (aa <= tover * alpem)
					aa = 0;

				b[n - 1] = aa + aa * bb / alpem;
				if (b[n - 1] == 0. && ncalc > n)
					ncalc = n - 1;
			}
		}
		}
	} else if (x > 25. && b.length <= intx + 1) {
		/* ------------------------------------------------------------
			Asymptotic series for X > 25 (and not too large b.length)
			------------------------------------------------------------ */
		xc = sqrt(pi2 / x);
		xin = 1 / (64 * x * x);
		if (x >= 130.)	m = 4;
		else if (x >= 35.) m = 8;
		else		m = 11;
		xm = 4. * cast(double) m;
		/* ------------------------------------------------
			Argument reduction for SIN and COS routines.
			------------------------------------------------ */
		t = trunc(x / (twopi1 + twopi2) + .5);
		z = (x - t * twopi1) - t * twopi2 - (nu + .5) / pi2;
		vsin = sin(z);
		vcos = cos(z);
		gnu = twonu;
		for (i = 1; i <= 2; ++i) {
			s = (xm - 1. - gnu) * (xm - 1. + gnu) * xin * .5;
			t = (gnu - (xm - 3.)) * (gnu + (xm - 3.));
			t1= (gnu - (xm + 1.)) * (gnu + (xm + 1.));
			k = m + m;
			capp = s * t / fact[k];
			capq = s * t1/ fact[k + 1];
			xk = xm;
			for (; k >= 4; k -= 2) {/* k + 2(j-2) == 2m */
				xk -= 4.;
				s = (xk - 1. - gnu) * (xk - 1. + gnu);
				t1 = t;
				t = (gnu - (xk - 3.)) * (gnu + (xk - 3.));
				capp = (capp + 1. / fact[k - 2]) * s * t  * xin;
				capq = (capq + 1. / fact[k - 1]) * s * t1 * xin;

			}
			capp += 1.;
			capq = (capq + 1.) * (gnu * gnu - 1.) * (.125 / x);
			b[i - 1] = xc * (capp * vcos - capq * vsin);
			if (b.length == 1)
				return;

			/* vsin <--> vcos */ t = vsin; vsin = -vcos; vcos = t;
			gnu += 2.;
		}
		/* -----------------------------------------------
			If  NB > 2, compute J(X,ORDER+I)	for I = 2, NB-1
			----------------------------------------------- */
		if (b.length > 2)
		for (gnu = twonu + 2., j = 2; j <= b.length; j++, gnu += 2.)
			b[j] = gnu * b[j - 1] / x - b[j - 2];
	}
	else {
		/* rtnsig_BESS <= x && ( x <= 25 || intx+1 < b.length ) :
			--------------------------------------------------------
			Use recurrence to generate results.
			First initialize the calculation of P*S.
			-------------------------------------------------------- */
		nbmx = b.length - intx;
		n = intx + 1;
		en = cast(double)(n + n) + twonu;
		plast = 1.;
		p = en / x;
		/* ---------------------------------------------------
			Calculate general significance test.
			--------------------------------------------------- */
		test = ensig_BESS + ensig_BESS;
		if (nbmx >= 3) {
		/* ------------------------------------------------------------
			Calculate P*S until N = NB-1.  Check for possible overflow.
			---------------------------------------------------------- */
		tover = enten_BESS / ensig_BESS;
		nstart = intx + 2;
		nend = b.length - 1;
		en = cast(double) (nstart + nstart) - 2. + twonu;
		for (k = nstart; k <= nend; ++k) {
			n = k;
			en += 2.;
			pold = plast;
			plast = p;
			p = en * plast / x - pold;
			if (p > tover) {
			/* -------------------------------------------
				To avoid overflow, divide P*S b TOVER.
				Calculate P*S until ABS(P) > 1.
				-------------------------------------------*/
			tover = enten_BESS;
			p /= tover;
			plast /= tover;
			psave = p;
			psavel = plast;
			nstart = n + 1;
			do {
				++n;
				en += 2.;
				pold = plast;
				plast = p;
				p = en * plast / x - pold;
			} while (p <= 1.);

			bb = en / x;
			/* -----------------------------------------------
				Calculate backward test and find NCALC,
				the highest N such that the test is passed.
				----------------------------------------------- */
			test = pold * plast * (.5 - .5 / (bb * bb));
			test /= ensig_BESS;
			p = plast * tover;
			--n;
			en -= 2.;
			nend = min(b.length,n);
			for (l = nstart; l <= nend; ++l) {
				pold = psavel;
				psavel = psave;
				psave = en * psavel / x - pold;
				if (psave * psavel > test) {
				ncalc = l - 1;
				goto L190;
				}
			}
			ncalc = nend;
			goto L190;
			}
		}
		n = nend;
		en = cast(double) (n + n) + twonu;
		/* -----------------------------------------------------
			Calculate special significance test for NBMX > 2.
			-----------------------------------------------------*/
		test = max(test, sqrt(plast * ensig_BESS) * sqrt(p + p));
		}
		/* ------------------------------------------------
			Calculate P*S until significance test passes. */
		do {
		++n;
		en += 2.;
		pold = plast;
		plast = p;
		p = en * plast / x - pold;
		} while (p < test);

L190:
		/*---------------------------------------------------------------
		  Initialize the backward recursion and the normalization sum.
		  --------------------------------------------------------------- */
		++n;
		en += 2.;
		bb = 0;
		aa = 1. / p;
		m = n / 2;
		em = cast(double)m;
		m = (n << 1) - (m << 2);/* = 2 n - 4 (n/2)
						= 0 for even, 2 for odd n */
		if (m == 0)
		sum = 0;
		else {
		alpem = em - 1. + nu;
		alp2em = em + em + nu;
		sum = aa * alpem * alp2em / em;
		}
		nend = n - b.length;
		/* if (nend > 0) */
		/* --------------------------------------------------------
			Recur backward via difference equation, calculating
			(but not storing) b[N], until N = NB.
			-------------------------------------------------------- */
		for (l = 1; l <= nend; ++l) {
		--n;
		en -= 2.;
		cc = bb;
		bb = aa;
		aa = en * bb / x - cc;
		m = m ? 0 : 2; /* m = 2 - m failed on gcc4-20041019 */
		if (m != 0) {
			em -= 1.;
			alp2em = em + em + nu;
			if (n == 1)
			break;

			alpem = em - 1. + nu;
			if (alpem == 0.)
			alpem = 1.;
			sum = (sum + aa * alp2em) * alpem / em;
		}
		}
		/*--------------------------------------------------
		  Store b[NB].
		  --------------------------------------------------*/
		b[n - 1] = aa;
		if (nend >= 0) {
		if (b.length <= 1) {
			if (nu + 1. == 1.)
			alp2em = 1.;
			else
			alp2em = nu;
			sum += b[0] * alp2em;
			goto L250;
		}
		else {/*-- b.length >= 2 : ---------------------------
			Calculate and store b[NB-1].
			----------------------------------------*/
			--n;
			en -= 2.;
			b[n - 1] = en * aa / x - bb;
			if (n == 1)
			goto L240;

			m = m ? 0 : 2; /* m = 2 - m failed on gcc4-20041019 */
			if (m != 0) {
			em -= 1.;
			alp2em = em + em + nu;
			alpem = em - 1. + nu;
			if (alpem == 0.)
				alpem = 1.;
			sum = (sum + b[n - 1] * alp2em) * alpem / em;
			}
		}
		}

		/* if (n - 2 != 0) */
		/* --------------------------------------------------------
			Calculate via difference equation and store b[N],
			until N = 2.
			-------------------------------------------------------- */
		for (n = n-1; n >= 2; n--) {
		en -= 2.;
		b[n - 1] = en * b[n + 0] / x - b[n + 1];
		m = m ? 0 : 2; /* m = 2 - m failed on gcc4-20041019 */
		if (m != 0) {
			em -= 1.;
			alp2em = em + em + nu;
			alpem = em - 1. + nu;
			if (alpem == 0.)
			alpem = 1.;
			sum = (sum + b[n - 1] * alp2em) * alpem / em;
		}
		}
		/* ---------------------------------------
			Calculate b[1].
			-----------------------------------------*/
		b[0] = 2. * (nu + 1.) * b[1] / x - b[2];

L240:
		em -= 1.;
		alp2em = em + em + nu;
		if (alp2em == 0.)
		alp2em = 1.;
		sum += b[0] * alp2em;

L250:
		/* ---------------------------------------------------
			Normalize.  Divide all b[N] b sum.
			---------------------------------------------------*/
/*		if (nu + 1. != 1.) poor test */
		if(fabs(nu) > 1e-15)
		sum *= (gamma_cody(nu) * pow(.5* x, -nu));

		aa = enmten_BESS;
		if (sum > 1.)
		aa *= sum;
		for (n = 1; n <= b.length; ++n) {
		if (fabs(b[n - 1]) < aa)
			b[n - 1] = 0;
		else
			b[n - 1] /= sum;
		}
	}

	}
	else {
	  /* Error return -- X, NB, or ALPHA is out of range : */
	b[0] = 0;
	ncalc = min(b.length, 0) - 1;
	}
}

///
double besselY(double x, double alpha)
{
	ptrdiff_t nb, ncalc;
	double na;
	double* b;

	/* NaNs propagated correctly */
	if (std.math.isNaN(x) || std.math.isNaN(alpha)) 
		return x + alpha;
	if (x < 0)
		return double.nan;
	na = floor(alpha);
	if (alpha < 0) {
	/* Using Aaamowitz & Stegun  9.1.2
	 * this may not be quite optimal (CPU and accuracy wise) */
	return(besselY(x, -alpha) * cosPi(alpha) -
			((alpha == na) ? 0 :
		besselJ(x, -alpha) * sinPi(alpha)));
	}
	nb = 1+ cast(int)na;/* b.length-1 <= alpha < b.length */
	alpha -= nb-1;
	b = cast(double*) calloc(nb, double.sizeof).enforce("besselY allocation error");
	scope(exit) b.free;
	besselY(x, alpha, b[0..nb], ncalc);
	if(ncalc != nb) {/* error input */
	if(ncalc == -1)
		return double.infinity;
	else if(ncalc < -1)
			throw new Exception(format("besselY(%g): ncalc (=%ld) != b.length (=%ld); alpha=%g. Arg. out of range?\n",
				 x, ncalc, nb, alpha));
	else /* ncalc >= 0 */
			throw new Exception(format("besselY(%g,nu=%g): precision lost in result\n",
				 x, alpha+(nb-1)));
	}
	x = b[nb-1];
	return x;
}


///
unittest {
	assert(std.math.approxEqual(besselY(2, 1), -0.1070324315409375468883707722774));
	assert(std.math.approxEqual(besselY(20, 1), -0.165511614362521295863976023243));
	assert(std.math.approxEqual(besselY(1, 1.2), -0.901215));
}

/* modified version of besselY that accepts a work array instead of
	allocating one. */
double besselY(double x, double alpha, double[] b)
{
	ptrdiff_t nb, ncalc;
	double na;

	/* NaNs propagated correctly */
	if (std.math.isNaN(x) || std.math.isNaN(alpha)) 
		return x + alpha;
	if (x < 0)
		return double.nan;
	na = floor(alpha);
	if (alpha < 0) {
	/* Using Aaamowitz & Stegun  9.1.2
	 * this may not be quite optimal (CPU and accuracy wise) */
	return(besselY(x, -alpha, b) * cosPi(alpha) -
			((alpha == na) ? 0 :
		besselJ(x, -alpha, b) * sinPi(alpha)));
	}
	nb = 1+ cast(int)na;/* b.length-1 <= alpha < b.length */
	alpha -= nb-1;
	besselY(x, alpha, b[0..nb], ncalc);
	if(ncalc != nb) {/* error input */
	if(ncalc == -1)
		return double.infinity;
	else if(ncalc < -1)
			throw new Exception(format("besselY(%g): ncalc (=%ld) != b.length (=%ld); alpha=%g. Arg. out of range?\n",
				 x, ncalc, nb, alpha));
	else /* ncalc >= 0 */
			throw new Exception(format("besselY(%g,nu=%g): precision lost in result\n",
				 x, alpha+(nb-1)));
	}
	x = b[nb-1];
	return x;
}


void besselY(double x, double alpha, double[] b, ref ptrdiff_t ncalc)
{
/* ----------------------------------------------------------------------

 This routine calculates Bessel functions Y_(N+ALPHA) (X)
v for non-negative argument X, and non-negative order N+ALPHA.


 Explanation of variables in the calling sequence

 X	 - Non-negative argument for which
	 Y's are to be calculated.
 ALPHA - Fractional part of order for which
	 Y's are to be calculated.  0 <= ALPHA < 1.0.
 NB	- Number of functions to be calculated, NB > 0.
	 The first function calculated is of order ALPHA, and the
	 last is of order (NB - 1 + ALPHA).
 BY	- Output vector of length NB.	If the
	 routine terminates normally (NCALC=NB), the vector BY
	 contains the functions Y(ALPHA,X), ... , Y(NB-1+ALPHA,X),
	 If (0 < NCALC < NB), BY(I) contains correct function
	 values for I <= NCALC, and contains the ratios
	 Y(ALPHA+I-1,X)/Y(ALPHA+I-2,X) for the rest of the array.
 NCALC - Output variable indicating possible errors.
	 Before using the vector BY, the user should check that
	 NCALC=NB, i.e., all orders have been calculated to
	 the desired accuracy.	See error returns below.


 *******************************************************************

 Error returns

  In case of an error, NCALC != NB, and not all Y's are
  calculated to the desired accuracy.

  NCALC < -1:  An argument is out of range. For example,
	NB <= 0, IZE is not 1 or 2, or IZE=1 and ABS(X) >=
	XMAX.  In this case, BY[0] = 0.0, the remainder of the
	BY-vector is not calculated, and NCALC is set to
	MIN0(NB,0)-2  so that NCALC != NB.
  NCALC = -1:  Y(ALPHA,X) >= XINF.  The requested function
	values are set to 0.0.
  1 < NCALC < NB: Not all requested function values could
	be calculated accurately.  BY(I) contains correct function
	values for I <= NCALC, and and the remaining NB-NCALC
	array elements contain 0.0.


 Intrinsic functions required are:

	 DBLE, EXP, INT, MAX, MIN, REAL, SQRT


 Acknowledgement

	This program draws heavily on Temme's Algol program for Y(a,x)
	and Y(a+1,x) and on Campbell's programs for Y_nu(x).	Temme's
	scheme is used for  x < THRESH, and Campbell's scheme is used
	in the asymptotic region.  Segments of code from both sources
	have been translated into Fortran 77, merged, and heavily modified.
	Modifications include parameterization of machine dependencies,
	use of a new approximation for ln(gamma(x)), and built-in
	protection against over/underflow.

 References: "Bessel functions J_nu(x) and Y_nu(x) of float
		  order and float argument," Campbell, J. B.,
		  Comp. Phy. Comm. 18, 1979, pp. 133-142.

		 "On the numerical evaluation of the ordinary
		  Bessel function of the second kind," Temme,
		  N. M., J. Comput. Phys. 21, 1976, pp. 343-350.

  Latest modification: March 19, 1990

  Modified b: W. J. Cody
			Applied Mathematics Division
			Argonne National Laboratory
			Argonne, IL  60439
 ----------------------------------------------------------------------*/


/* ----------------------------------------------------------------------
  Mathematical constants
	FIVPI = 5*PI
	PIM5 = 5*PI - 15
 ----------------------------------------------------------------------*/
	static immutable double fivpi = 15.707963267948966192;
	static immutable double pim5	=	.70796326794896619231;

	/*----------------------------------------------------------------------
	  Coefficients for Chebshev polynomial expansion of
	  1/gamma(1-x), abs(x) <= .5
	  ----------------------------------------------------------------------*/
	static immutable double[21] ch = [ -6.7735241822398840964e-24,
		-6.1455180116049879894e-23,2.9017595056104745456e-21,
		1.3639417919073099464e-19,2.3826220476859635824e-18,
		-9.0642907957550702534e-18,-1.4943667065169001769e-15,
		-3.3919078305362211264e-14,-1.7023776642512729175e-13,
		9.1609750938768647911e-12,2.4230957900482704055e-10,
		1.7451364971382984243e-9,-3.3126119768180852711e-8,
		-8.6592079961391259661e-7,-4.9717367041957398581e-6,
		7.6309597585908126618e-5,.0012719271366545622927,
		.0017063050710955562222,-.07685284084478667369,
		-.28387654227602353814,.92187029365045265648 ];

	/* Local variables */
	ptrdiff_t i, k, na;

	double alfa, div, ddiv, even, gamma, term, cosmu, sinmu,
	a, c, d, e, f, g, h, p, q, r, s, d1, d2, q0, pa,pa1, qa,qa1,
	en, en1, nu, ex,  ya,ya1, twobx, den, odd, aye, dmu, x2, xna;

	en1 = ya = ya1 = 0;		/* -Wall */

	ex = x;
	nu = alpha;
	if (b.length > 0 && 0. <= nu && nu < 1.) {
	if(ex < double.min_normal || ex > xlrg_BESS_Y) {
		/* Warning is not really appropriate, give
		 * proper limit:
		 * ML_ERROR(ME_RANGE, "besselY"); */
		ncalc = b.length;
		if(ex > xlrg_BESS_Y)  b[0]= 0; /*was double.infinity */
		else if(ex < double.min_normal) b[0]=-double.infinity;
		for(i=0; i < b.length; i++)
		b[i] = b[0];
		return;
	}
	xna = trunc(nu + .5);
	na = cast(int) xna;
	if (na == 1) {/* <==>  .5 <= alpha < 1	 <==>  -5. <= nu < 0 */
		nu -= xna;
	}
	if (nu == -.5) {
		p = M_SQRT_2dPI / sqrt(ex);
		ya = p * sin(ex);
		ya1 = -p * cos(ex);
	} else if (ex < 3.) {
		/* -------------------------------------------------------------
			Use Temme's scheme for small X
			------------------------------------------------------------- */
		a = ex * .5;
		d = -log(a);
		f = nu * d;
		e = pow(a, -nu);
		if (fabs(nu) < M_eps_sinc)
		c = std.math.M_1_PI;
		else
		c = nu / sinPi(nu);

		/* ------------------------------------------------------------
			Computation of sinh(f)/f
			------------------------------------------------------------ */
		if (fabs(f) < 1.) {
		x2 = f * f;
		en = 19.;
		s = 1.;
		for (i = 1; i <= 9; ++i) {
			s = s * x2 / en / (en - 1.) + 1.;
			en -= 2.;
		}
		} else {
		s = (e - 1. / e) * .5 / f;
		}
		/* --------------------------------------------------------
			Computation of 1/gamma(1-a) using Chebshev polynomials */
		x2 = nu * nu * 8.;
		aye = ch[0];
		even = 0;
		alfa = ch[1];
		odd = 0;
		for (i = 3; i <= 19; i += 2) {
		even = -(aye + aye + even);
		aye = -even * x2 - aye + ch[i - 1];
		odd = -(alfa + alfa + odd);
		alfa = -odd * x2 - alfa + ch[i];
		}
		even = (even * .5 + aye) * x2 - aye + ch[20];
		odd = (odd + alfa) * 2.;
		gamma = odd * nu + even;
		/* End of computation of 1/gamma(1-a)
			----------------------------------------------------------- */
		g = e * gamma;
		e = (e + 1. / e) * .5;
		f = 2. * c * (odd * e + even * s * d);
		e = nu * nu;
		p = g * c;
		q = std.math.M_1_PI / g;
		c = nu * std.math.PI_2;
		if (fabs(c) < M_eps_sinc)
		r = 1.;
		else
		r = sinPi(nu/2) / c;

		r = cast(double)std.math.PI * c * r * r;
		c = 1.;
		d = -(a * a);
		h = 0;
		ya = f + r * q;
		ya1 = p;
		en = 1.;

		while (fabs(g / (1. + fabs(ya))) +
			fabs(h / (1. + fabs(ya1))) > double.epsilon) {
		f = (f * en + p + q) / (en * en - e);
		c *= (d / en);
		p /= en - nu;
		q /= en + nu;
		g = c * (f + r * q);
		h = c * p - en * g;
		ya += g;
		ya1+= h;
		en += 1.;
		}
		ya = -ya;
		ya1 = -ya1 / a;
	} else if (ex < thresh_BESS_Y) {
		/* --------------------------------------------------------------
			Use Temme's scheme for moderate X :  3 <= x < 16
			-------------------------------------------------------------- */
		c = (.5 - nu) * (.5 + nu);
		a = ex + ex;
		e = ex * std.math.M_1_PI * cosPi(nu) / double.epsilon;
		e *= e;
		p = 1.;
		q = -ex;
		r = 1. + ex * ex;
		s = r;
		en = 2.;
		while (r * en * en < e) {
		en1 = en + 1.;
		d = (en - 1. + c / en) / s;
		p = (en + en - p * d) / en1;
		q = (-a + q * d) / en1;
		s = p * p + q * q;
		r *= s;
		en = en1;
		}
		f = p / s;
		p = f;
		g = -q / s;
		q = g;
L220:
		en -= 1.;
		if (en > 0.) {
		r = en1 * (2. - p) - 2.;
		s = a + en1 * q;
		d = (en - 1. + c / en) / (r * r + s * s);
		p = d * r;
		q = d * s;
		e = f + 1.;
		f = p * e - g * q;
		g = q * e + p * g;
		en1 = en;
		goto L220;
		}
		f = 1. + f;
		d = f * f + g * g;
		pa = f / d;
		qa = -g / d;
		d = nu + .5 - p;
		q += ex;
		pa1 = (pa * q - qa * d) / ex;
		qa1 = (qa * q + pa * d) / ex;
		a = ex - std.math.PI_2 * (nu + .5);
		c = cos(a);
		s = sin(a);
		d = M_SQRT_2dPI / sqrt(ex);
		ya = d * (pa * s + qa * c);
		ya1 = d * (qa1 * s - pa1 * c);
	} else { /* x > thresh_BESS_Y */
		/* ----------------------------------------------------------
			Use Campbell's asymptotic scheme.
			---------------------------------------------------------- */
		na = 0;
		d1 = trunc(ex / fivpi);
		i = cast(int) d1;
		dmu = ex - 15. * d1 - d1 * pim5 - (alpha + .5) * std.math.PI_2;
		if (i - (i / 2 << 1) == 0) {
		cosmu = cos(dmu);
		sinmu = sin(dmu);
		} else {
		cosmu = -cos(dmu);
		sinmu = -sin(dmu);
		}
		ddiv = 8. * ex;
		dmu = alpha;
		den = sqrt(ex);
		for (k = 1; k <= 2; ++k) {
		p = cosmu;
		cosmu = sinmu;
		sinmu = -p;
		d1 = (2. * dmu - 1.) * (2. * dmu + 1.);
		d2 = 0;
		div = ddiv;
		p = 0;
		q = 0;
		q0 = d1 / div;
		term = q0;
		for (i = 2; i <= 20; ++i) {
			d2 += 8.;
			d1 -= d2;
			div += ddiv;
			term = -term * d1 / div;
			p += term;
			d2 += 8.;
			d1 -= d2;
			div += ddiv;
			term *= (d1 / div);
			q += term;
			if (fabs(term) <= double.epsilon) {
			break;
			}
		}
		p += 1.;
		q += q0;
		if (k == 1)
			ya = M_SQRT_2dPI * (p * cosmu - q * sinmu) / den;
		else
			ya1 = M_SQRT_2dPI * (p * cosmu - q * sinmu) / den;
		dmu += 1.;
		}
	}
	if (na == 1) {
		h = 2. * (nu + 1.) / ex;
		if (h > 1.) {
		if (fabs(ya1) > double.max / h) {
			h = 0;
			ya = 0;
		}
		}
		h = h * ya1 - ya;
		ya = ya1;
		ya1 = h;
	}

	/* ---------------------------------------------------------------
		Now have first one or two Y's
		--------------------------------------------------------------- */
	b[0] = ya;
	ncalc = 1;
	if(b.length > 1) {
		b[1] = ya1;
		if (ya1 != 0.) {
		aye = 1. + alpha;
		twobx = 2. / ex;
		ncalc = 2;
		for (i = 2; i < b.length; ++i) {
			if (twobx < 1.) {
			if (fabs(b[i - 1]) * twobx >= double.max / aye)
				goto L450;
			} else {
			if (fabs(b[i - 1]) >= double.max / aye / twobx)
				goto L450;
			}
			b[i] = twobx * aye * b[i - 1] - b[i - 2];
			aye += 1.;
			ncalc++;
		}
		}
	}
L450:
	for (i = ncalc; i < b.length; ++i)
		b[i] = -double.infinity;/* was 0 */

	} else {
	b[0] = 0;
	ncalc = min(b.length,0) - 1;
	}
}


private:

double gamma_cody(double x)
{
/* ----------------------------------------------------------------------

	This routine calculates the GAMMA function for a float argument X.
	Computation is based on an algorithm outlined in reference [1].
	The program uses rational functions that approximate the GAMMA
	function to at least 20 significant decimal digits.	Coefficients
	for the approximation over the interval (1,2) are unpublished.
	Those for the approximation for X >= 12 are from reference [2].
	The accuracy achieved depends on the arithmetic system, the
	compiler, the intrinsic functions, and proper selection of the
	machine-dependent constants.

	*******************************************************************

	Error returns

	The program returns the value XINF for singularities or
	when overflow would occur.	 The computation is believed
	to be free of underflow and overflow.

	Intrinsic functions required are:

	INT, DBLE, EXP, LOG, REAL, SIN


	References:
	[1]  "An Overview of Software Development for Special Functions",
	W. J. Cody, Lecture Notes in Mathematics, 506,
	Numerical Analysis Dundee, 1975, G. A. Watson (ed.),
	Springer Verlag, Berlin, 1976.

	[2]  Computer Approximations, Hart, Et. Al., Wiley and sons, New York, 1968.

	Latest modification: October 12, 1989

	Authors: W. J. Cody and L. Stoltz
	Applied Mathematics Division
	Argonne National Laboratory
	Argonne, IL 60439
	----------------------------------------------------------------------*/

/* ----------------------------------------------------------------------
	Mathematical constants
	----------------------------------------------------------------------*/
	enum double sqrtpi = .9189385332046727417803297; /* == ??? */

/* *******************************************************************

	Explanation of machine-dependent constants

	beta	- radix for the floating-point representation
	maxexp - the smallest positive power of beta that overflows
	XBIG	- the largest argument for which GAMMA(X) is representable
	in the machine, i.e., the solution to the equation
	GAMMA(XBIG) = beta**maxexp
	XINF	- the largest machine representable floating-point number;
	approximately beta**maxexp
	EPS	- the smallest positive floating-point number such that  1.0+EPS > 1.0
	XMININ - the smallest positive floating-point number such that
	1/XMININ is machine representable

	Approximate values for some important machines are:

	beta		  maxexp		 XBIG

	CRAY-1		(S.P.)		  2		8191		966.961
	Cyber 180/855
	under NOS	(S.P.)		  2		1070		177.803
	IEEE (IBM/XT,
	SUN, etc.)	(S.P.)		  2		 128		35.040
	IEEE (IBM/XT,
	SUN, etc.)	(D.P.)		  2		1024		171.624
	IBM 3033	(D.P.)		 16		  63		57.574
	VAX D-Format	(D.P.)		  2		 127		34.844
	VAX G-Format	(D.P.)		  2		1023		171.489

	XINF	 EPS		XMININ

	CRAY-1		(S.P.)	 5.45E+2465	7.11E-15	  1.84E-2466
	Cyber 180/855
	under NOS	(S.P.)	 1.26E+322	3.55E-15	  3.14E-294
	IEEE (IBM/XT,
	SUN, etc.)	(S.P.)	 3.40E+38	 1.19E-7	  1.18E-38
	IEEE (IBM/XT,
	SUN, etc.)	(D.P.)	 1.79D+308	2.22D-16	  2.23D-308
	IBM 3033	(D.P.)	 7.23D+75	 2.22D-16	  1.39D-76
	VAX D-Format	(D.P.)	 1.70D+38	 1.39D-17	  5.88D-39
	VAX G-Format	(D.P.)	 8.98D+307	1.11D-16	  1.12D-308

	*******************************************************************

	----------------------------------------------------------------------
	Machine dependent parameters
	----------------------------------------------------------------------
	*/


	enum double xbg = 171.624;
	/* ML_POSINF ==	const double xinf = 1.79e308;*/
	/* DBL_EPSILON = const double eps = 2.22e-16;*/
	/* DBL_MIN ==	const double xminin = 2.23e-308;*/

	/*----------------------------------------------------------------------
	  Numerator and denominator coefficients for rational minimax
	  approximation over (1,2).
	  ----------------------------------------------------------------------*/
	static immutable double[8] p = [
	-1.71618513886549492533811,
	24.7656508055759199108314,-379.804256470945635097577,
	629.331155312818442661052,866.966202790413211295064,
	-31451.2729688483675254357,-36144.4134186911729807069,
	66456.1438202405440627855 ];
	static immutable double[8] q = [
	-30.8402300119738975254353,
	315.350626979604161529144,-1015.15636749021914166146,
	-3107.77167157231109440444,22538.1184209801510330112,
	4755.84627752788110767815,-134659.959864969306392456,
	-115132.259675553483497211 ];
	/*----------------------------------------------------------------------
	  Coefficients for minimax approximation over (12, INF).
	  ----------------------------------------------------------------------*/
	static immutable double[7] c = [
	-.001910444077728,8.4171387781295e-4,
	-5.952379913043012e-4,7.93650793500350248e-4,
	-.002777777777777681622553,.08333333333333333331554247,
	.0057083835261 ];

	/* Local variables */
	int i, n;
	int parity;/*logical*/
	double fact, xden, xnum, y, z, yi, res, sum, ysq;

	parity = (0);
	fact = 1.;
	n = 0;
	y = x;
	if (y <= 0.) {
	/* -------------------------------------------------------------
		Argument is negative
		------------------------------------------------------------- */
	y = -x;
	yi = trunc(y);
	res = y - yi;
	if (res != 0.) {
		if (yi != trunc(yi * .5) * 2.)
		parity = (1);
		fact = -std.math.PI / sinPi(res);
		y += 1.;
	} else {
		return(double.infinity);
	}
	}
	/* -----------------------------------------------------------------
		Argument is positive
		-----------------------------------------------------------------*/
	if (y < double.epsilon) {
	/* --------------------------------------------------------------
		Argument < EPS
		-------------------------------------------------------------- */
	if (y >= double.min_normal) {
		res = 1. / y;
	} else {
		return(double.infinity);
	}
	} else if (y < 12.) {
	yi = y;
	if (y < 1.) {
		/* ---------------------------------------------------------
			EPS < argument < 1
			--------------------------------------------------------- */
		z = y;
		y += 1.;
	} else {
		/* -----------------------------------------------------------
			1 <= argument < 12, reduce argument if necessary
			----------------------------------------------------------- */
		n = cast(int) y - 1;
		y -= cast(double) n;
		z = y - 1.;
	}
	/* ---------------------------------------------------------
		Evaluate approximation for 1. < argument < 2.
		---------------------------------------------------------*/
	xnum = 0;
	xden = 1.;
	for (i = 0; i < 8; ++i) {
		xnum = (xnum + p[i]) * z;
		xden = xden * z + q[i];
	}
	res = xnum / xden + 1.;
	if (yi < y) {
		/* --------------------------------------------------------
			Adjust result for case  0. < argument < 1.
			-------------------------------------------------------- */
		res /= yi;
	} else if (yi > y) {
		/* ----------------------------------------------------------
			Adjust result for case  2. < argument < 12.
			---------------------------------------------------------- */
		for (i = 0; i < n; ++i) {
		res *= y;
		y += 1.;
		}
	}
	} else {
	/* -------------------------------------------------------------
		Evaluate for argument >= 12.,
		------------------------------------------------------------- */
	if (y <= xbg) {
		ysq = y * y;
		sum = c[6];
		for (i = 0; i < 6; ++i) {
		sum = sum / ysq + c[i];
		}
		sum = sum / y - y + sqrtpi;
		sum += (y - .5) * log(y);
		res = exp(sum);
	} else {
		return double.infinity;
	}
	}
	/* ----------------------------------------------------------------------
		Final adjustments and return
		----------------------------------------------------------------------*/
	if (parity)
	res = -res;
	if (fact != 1.)
	res = fact / res;
	return res;
}

/// sin of pix:
T sinPi(T)(T x)
	if(isFloatingPoint!T)
{
	if(x < 0)
	  return -sinPi(-x);
	// sin of pix:
	bool invert;
	if(x < cast(T)0.5)
	  return sin(T(std.math.PI) * x);
	if(x < 1)
	{
	  invert = true;
	  x = -x;
	}
	else
	  invert = false;

	T rem = floor(x);
	if(cast(int)rem & 1)
	  invert = !invert;
	rem = x - rem;
	if(rem > cast(T)0.5)
	  rem = 1 - rem;
	if(rem == cast(T)0.5)
	  return invert ? -1 : 1;
	
	rem = sin(T(std.math.PI) * rem);
	return invert ? T(-rem) : rem;
}

unittest 
{
	import std.math : approxEqual;
	assert(sinPi(0.0) == 0);
	assert(sinPi(1.0) == 0);
	assert(sinPi(+1.0/6).approxEqual(+0.5));
	assert(sinPi(-1.0/6).approxEqual(-0.5));
	assert(sinPi(+5.0/6).approxEqual(+0.5));
	assert(sinPi(-5.0/6).approxEqual(-0.5));
	assert(sinPi(+2.0) == 0);
	assert(sinPi(-2.0) == 0);
	assert(sinPi(+0.5) == +1);
	assert(sinPi(-0.5) == -1);
}


/// cos of pix:
T cosPi(T)(T x)
	if(isFloatingPoint!T)
{
	// cos of pix:
	bool invert = false;
	if(fabs(x) < 0.5)
	  return cos(T(std.math.PI) * x);

	if(x < cast(T)1)
	{
	  x = -x;
	}
	T rem = floor(x);
	if(cast(int)rem & 1)
	  invert = !invert;
	rem = x - rem;
	if(rem > cast(T)0.5)
	{
	  rem = 1 - rem;
	  invert = !invert;
	}
	if(rem == cast(T)0.5)
	  return 0;
	
	rem = cos(T(std.math.PI) * rem);
	return invert ? T(-rem) : rem;
}


unittest
{
	import std.math : approxEqual;
	assert(cosPi(0.0) == 1);
	assert(cosPi(1.0) == -1);
	assert(cosPi(+1.0/3).approxEqual(+0.5));
	assert(cosPi(-1.0/3).approxEqual(+0.5));
	assert(cosPi(+2.0/3).approxEqual(-0.5));
	assert(cosPi(-2.0/3).approxEqual(-0.5));
	assert(cosPi(2.0) == 1);
	assert(cosPi(0.5) == 0);
}
