// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open МатКлассы
open System.IO
open System
open System.Threading.Tasks

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
let mutable x0=0.
let mutable c1,c2,omega,ro,k1,k2,mu1,mu2=0.,0.,0.,0.,0.,0.,0.,0.
let mutable xmin,xmax,ymin,ymax,xcount,ycount=0.,1.,4.,1.,40,40

let mutable t1=0.
let mutable t2=0.
let mutable tm=0.

let mutable eps=1e-4
let mutable pr=0.01
let mutable gr=1e4

//функция, отвечающая за радикал, который может обрываться, из-за чего надо именно такую формулу использовать
let sigmak kw (a:Number.Complex) =
    if a.Abs > kw then Number.Complex.Sqrt((a - kw) * (a + kw))
    else
        let tmp = Number.Complex.I * Number.Complex.Sqrt((kw - a) * (kw + a))
        if a.Im * a.Re > 0. then tmp
        else -tmp


[<EntryPoint>]
let main argv = 
   // значения параметров из файла
    let params = File.ReadAllLines "Конфигурация.txt" |> Array.map (fun s-> (s.Split [|' '|]).[1] |> Convert.ToDouble)
    
    Array.iter( fun p -> printf "%f \n" p) params


    k1<-c1/omega
    k2<-c2/omega
    let sigma1=sigmak k1
    let sigma2=sigmak k2


    //функции

    //tex:$Qe(alpha,x)=Q(\alpha)*exp(-i \alpha x)=exp(-i \alpha (x_0-x))$
    let Qexp (alpha:Number.Complex) x = Number.Complex.Exp(Number.Complex(0.,-1.0)*alpha*(x0-x))
    let Delta alpha=0
    let K1 (alpha:Number.Complex) (z:float)  =alpha/alpha.Abs*z
    let K2 (alpha:Number.Complex) (z:float)  =z*alpha.Arg

    let u x z=
        if z > -h1 then FuncMethods.DefInteg.GaussKronrod.DINN_GK( (fun alpha->(K1 alpha z) * (Qexp alpha x)), t1, t1, t1, t2, tm ,0., eps, pr, gr)
        else FuncMethods.DefInteg.GaussKronrod.DINN_GK( (fun alpha->(K2 alpha z) * (Qexp alpha x)), t1, t1, t1, t2, tm ,0., eps, pr, gr)
    
    let values =Array2D.init xcount ycount (fun i j -> Number.Complex(0.))

    let xnet=Expendator.Seq(xmin,xmax,xcount)
    let ynet=Expendator.Seq(ymin,ymax,ycount)

    for i in 0..xcount-1 do
       Parallel.For(0, ycount, 
           fun j-> values.[i,j]<- (u xnet.[i] ynet.[j]))|> ignore
       printf "%u from %u \n" (i+1) xcount
       
            

    0
