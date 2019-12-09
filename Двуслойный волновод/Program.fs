// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open МатКлассы
open System.IO
open System


//глобальные переменные
let mutable a=1.0
let mutable b=1.0
let mutable c=1.0
let mutable d=1.0
let mutable e=1.0
let mutable f=1.0
let mutable g=1.0
let mutable h1=1.0
let mutable h2=2.0
let mutable x0=0.0

let mutable t1=0.0
let mutable t2=0.0
let mutable tm=0.0

let mutable eps=1e-4
let mutable pr=0.01
let mutable gr=1e4


[<EntryPoint>]
let main argv = 
   // значения параметров из файла
    let params = File.ReadAllLines "Конфигурация.txt" |> Array.map (fun s-> (s.Split [|' '|]).[1] |> Convert.ToDouble)
    
    Array.iter( fun p -> printf "%f \n" p) params




    //функции

    //tex:$Qe(alpha,x)=Q(\alpha)*exp(-i \alpha x)=exp(-i \alpha (x_0-x))$
    let Qexp (alpha:Number.Complex) x = Number.Complex.Exp(Number.Complex(0.0,-1.0)*alpha*(x0-x))
    let Delta alpha=0
    let K1 (alpha:Number.Complex) (z:float)  =alpha*z
    let K2 (alpha:Number.Complex) (z:float)  =z*alpha

    let u x z=
        if z > -h1 then FuncMethods.DefInteg.GaussKronrod.DINN_GK( (fun alpha->(K1 alpha z) * (Qexp alpha x)), t1, t1, t1, t2, tm ,0.0, eps, pr, gr)
        else FuncMethods.DefInteg.GaussKronrod.DINN_GK( (fun alpha->(K2 alpha z) * (Qexp alpha x)), t1, t1, t1, t2, tm ,0.0, eps, pr, gr)
    
    0
