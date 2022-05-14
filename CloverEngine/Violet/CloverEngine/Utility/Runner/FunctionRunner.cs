﻿﻿﻿﻿﻿using System;

 namespace Clover
{
    public delegate void Run();

    public delegate void Run<in T>(T p);

    public delegate void Run<in T1, in T2>(T1 p1, T2 p2);

    public delegate void Run<in T1, in T2, in T3>(T1 p1, T2 p2, T3 p3);

    public delegate void Run<in T1, in T2, in T3, in T4>(T1 p, T2 p2, T3 p3, T4 p4);

    public static class Function
    {
        public static bool Run(Run run)
        {
            try
            {
                run.Invoke();
            }
            catch (Exception ex)
            {
                Log.LogException(ex);
                return false;
            }

            return true;
        }

        public static bool Run(out Exception except, Run run)
        {
            try
            {
                run.Invoke();
            }
            catch (Exception ex)
            {
                except = ex;
                return (false);
            }

            except = null;

            return true;
        }

        public static bool Run<T>(Run<T> run, T p)
        {
            try
            {
                run.Invoke(p);
            }
            catch (Exception ex)
            {
                Log.LogException(ex);
                return false;
            }

            return true;
        }


        public static bool Run<T>(out Exception except, Run<T> run, T p)
        {
            try
            {
                run.Invoke(p);
            }
            catch (Exception ex)
            {
                except = ex;
                return false;
            }

            except = null;

            return true;
        }

        public static bool Run<T1, T2>(Run<T1, T2> run, T1 p1, T2 p2)
        {
            try
            {
                run.Invoke(p1, p2);
            }
            catch (Exception ex)
            {
                Log.LogException(ex);
                return false;
            }

            return true;
        }


        public static bool Run<T1, T2>(out Exception except, Run<T1, T2> run, T1 p1, T2 p2)
        {
            try
            {
                run.Invoke(p1, p2);
            }
            catch (Exception ex)
            {
                except = ex;
                return false;
            }

            except = null;

            return true;
        }

        public static bool Run<T1, T2, T3>(Run<T1, T2, T3> run, T1 p1, T2 p2, T3 p3)
        {
            try
            {
                run.Invoke(p1, p2, p3);
            }
            catch (Exception ex)
            {
                Log.LogException(ex);
                return false;
            }

            return true;
        }


        public static bool Run<T1, T2, T3>(out Exception except, Run<T1, T2, T3> run, T1 p1, T2 p2, T3 p3)
        {
            try
            {
                run.Invoke(p1, p2, p3);
            }
            catch (Exception ex)
            {
                except = ex;
                return false;
            }

            except = null;

            return true;
        }

        public static bool Run<T1, T2, T3, T4>(Run<T1, T2, T3, T4> run, T1 p1, T2 p2, T3 p3, T4 p4)
        {
            try
            {
                run.Invoke(p1, p2, p3, p4);
            }
            catch (Exception ex)
            {
                Log.LogException(ex);
                return false;
            }

            return true;
        }


        public static bool Run<T1, T2, T3, T4>(out Exception except, Run<T1, T2, T3, T4> run, T1 p1, T2 p2,
            T3 p3, T4 p4)
        {
            try
            {
                run.Invoke(p1, p2, p3, p4);
            }
            catch (Exception ex)
            {
                except = ex;
                return false;
            }

            except = null;

            return true;
        }
    }
}