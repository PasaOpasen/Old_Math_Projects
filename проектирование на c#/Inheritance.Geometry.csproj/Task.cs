using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance.Geometry
{
   public interface IVisitor
    {
        void Visit(Ball b);
        void Visit(Cube b);
        void Visit(Cyllinder b);
    }
	public abstract class Body
	{
        public abstract double GetVolume();
        public abstract void Accept(IVisitor visitor);
    }

	public class Ball : Body
	{
		public double Radius { get; set; }

        public override double GetVolume()
        {
            return 4.0 * Math.PI * Math.Pow(this.Radius, 3) / 3;
        }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

	public class Cube : Body
	{
		public double Size { get; set; }

        public override double GetVolume()
        {
            return Math.Pow(this.Size, 3);
        }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

	public class Cyllinder : Body
	{
		public double Height { get; set; }
		public double Radius { get; set; }

        public override double GetVolume()
        {
            return Math.PI * Math.Pow(this.Radius, 2) * this.Height;
        }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

	public class SurfaceAreaVisitor:IVisitor
	{
		public double SurfaceArea { get; private set; }

        void IVisitor.Visit(Ball b)
        {
            SurfaceArea = 4*Math.PI*b.Radius*b.Radius;
        }

        void IVisitor.Visit(Cube b)
        {
            SurfaceArea = 6*b.Size*b.Size;
        }

        void IVisitor.Visit(Cyllinder b)
        {
            SurfaceArea = 2*Math.PI*b.Radius*(b.Height+2);
        }
    }
	public class DimensionsVisitor : IVisitor
    {
		public Dimensions Dimensions { get; private set; }

        void IVisitor.Visit(Ball b)
        {
            Dimensions = new Dimensions(b.Radius *2, b.Radius * 2);
        }

        void IVisitor.Visit(Cube b)
        {
            Dimensions = new Dimensions(b.Size, b.Size);
        }

        void IVisitor.Visit(Cyllinder b)
        {
            Dimensions = new Dimensions(b.Radius*2,b.Height );
        }
    }
}
