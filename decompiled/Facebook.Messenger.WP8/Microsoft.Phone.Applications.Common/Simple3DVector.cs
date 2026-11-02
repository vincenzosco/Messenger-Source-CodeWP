using System;

namespace Microsoft.Phone.Applications.Common
{
	public class Simple3DVector
	{
		private double? _magnitude = null;

		public double X { get; private set; }

		public double Y { get; private set; }

		public double Z { get; private set; }

		public double Magnitude
		{
			get
			{
				if (!_magnitude.HasValue)
				{
					_magnitude = Math.Sqrt(X * X + Y * Y + Z * Z);
				}
				return _magnitude.Value;
			}
		}

		public Simple3DVector()
		{
		}

		public Simple3DVector(double x, double y, double z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public Simple3DVector(Simple3DVector v)
		{
			if (v != null)
			{
				X = v.X;
				Y = v.Y;
				Z = v.Z;
			}
		}

		public override string ToString()
		{
			return string.Format("({0:0.000},{1:0.000},{2:0.000})", X, Y, Z);
		}

		public static bool operator ==(Simple3DVector v1, Simple3DVector v2)
		{
			if (object.ReferenceEquals(v1, v2))
			{
				return true;
			}
			if ((object)v1 == null || (object)v2 == null)
			{
				return false;
			}
			if (v1.X == v2.X && v1.Y == v2.Y)
			{
				return v1.Z == v2.Z;
			}
			return false;
		}

		public static bool operator !=(Simple3DVector v1, Simple3DVector v2)
		{
			return !(v1 == v2);
		}

		public override bool Equals(object o)
		{
			if (o is Simple3DVector)
			{
				return this == (Simple3DVector)o;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
		}

		public static Simple3DVector operator +(Simple3DVector v1, Simple3DVector v2)
		{
			return new Simple3DVector(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
		}

		public static Simple3DVector operator -(Simple3DVector v1, Simple3DVector v2)
		{
			return new Simple3DVector(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
		}

		public static Simple3DVector operator *(Simple3DVector v1, Simple3DVector v2)
		{
			return new Simple3DVector(v1.X * v2.X, v1.Y * v2.Y, v1.Z * v2.Z);
		}

		public static Simple3DVector operator *(Simple3DVector v, double d)
		{
			return new Simple3DVector(d * v.X, d * v.Y, d * v.Z);
		}

		public static Simple3DVector operator /(Simple3DVector v, double d)
		{
			return new Simple3DVector(v.X / d, v.Y / d, v.Z / d);
		}
	}
}
