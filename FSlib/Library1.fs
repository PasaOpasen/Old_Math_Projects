
namespace FSlib

open System.Runtime.Remoting.Metadata.W3cXsd2001
 

type Class1() = 
    member this.X = "F#"

module testforbook=
   let p (x:double)=x**6.0+0.4*x**5.0-5.05*x**4.0-2.0*x**3.0+4.25*x**2.0+1.6*x-0.2
