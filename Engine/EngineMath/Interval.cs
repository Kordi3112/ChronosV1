
using System;

namespace Engine.EngineMath
{
    public struct Interval
    {
        public float Left { get; set; }
        public float Right { get; set; }


        bool isEmpty { get; set; }

        public static Interval Empty => new Interval() { Left = 0, Right = 0, isEmpty = true };

        public Interval(float left, float right)
        {
            Left = left;
            Right = right;

            isEmpty = false;

        }

        /// <summary>
        /// Right - Left
        /// </summary>
        public float Lenght()
        {
            return Right - Left;
        }


        public static bool Intersects(Interval A, Interval B)
        {
            if (A.Left < B.Left && A.Left < B.Right && A.Right < B.Left && A.Right < B.Right)
                return false;
            else if (B.Left < A.Left && B.Left < A.Right && B.Right < A.Left && B.Right < A.Right)
                return false;

            else return true;
        }

        public static Interval CommonPart(Interval A, Interval B)
        {
            if (A.Left < B.Left)
            {
                if (B.Left < A.Right)
                {
                    if (B.Right < A.Right)
                    {
                        //  -----               B
                        //----------            A

                        //  --------            B
                        //----------            A

                        return B;
                    }
                    else
                    {
                        //  ------------        B
                        //----------            A

                        return new Interval(B.Left, A.Right);
                    }
                }
                else if (B.Left > A.Right)
                {
                    //             ----------
                    //----------

                    return Empty;
                }
                else //B.Left == A.Right
                {
                    //         --------
                    //----------

                    return new Interval(A.Right, B.Left);
                }

            }
            else if (A.Left > B.Left)
            {
                if (B.Right < A.Right)
                {

                    if (B.Right < A.Left)
                    {
                        //    -
                        //      ----------

                        return Empty;
                    }
                    else if (B.Right == A.Left)
                    {
                        //  -----           B
                        //      ----------  A

                        return new Interval(B.Right, A.Left);
                    }
                    else
                    {
                        //    ---------
                        //      ----------

                        return new Interval(A.Left, B.Right);
                    }

                }
                else if (B.Right > A.Right)
                {
                    //  -----------            B
                    //      -----              A

                    return A;
                }
                else //B.Right == A.Right
                {
                    //  -------
                    //    -----

                    return A;
                }
            }
            else //A.Left == B.Left
            {
                if (B.Right <= A.Right)
                {
                    //-------                B
                    //----------             A

                    //----------             B
                    //----------             A

                    return B;

                }
                else //B.Left > A.Right
                {
                    //  -----------
                    //  --------

                    return A;
                }
            }
        }


        public static bool operator ==(Interval A, Interval B)
        {
            if (A.Left == B.Left && A.Right == B.Right && A.isEmpty == B.isEmpty)
                return true;
            else return false;
        }

        public static bool operator !=(Interval A, Interval B)
        {
            return !(A == B);
        }


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            if (isEmpty)
                return "EMPTY";
            else return "<" + Left + "," + Right + ">";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Interval))
            {
                return false;
            }

            var interval = (Interval)obj;

            return Left == interval.Left &&
                   Right == interval.Right &&
                   isEmpty == interval.isEmpty;
        }
    }
}
