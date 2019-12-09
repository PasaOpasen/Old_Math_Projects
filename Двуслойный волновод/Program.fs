// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open МатКлассы
open System.IO
open System


[<EntryPoint>]
let main argv = 
    let params = File.ReadAllLines "Конфигурация.txt" |> Array.map (fun s-> (s.Split [|' '|]).[1] |> Convert.ToDouble)
    
    Array.iter( fun p -> printf "%f \n" p) params

    
    0
