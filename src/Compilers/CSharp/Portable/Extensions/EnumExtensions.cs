
using System;
using System.Collections;

public static class _Enum
{
    public static bool DoesNotEqual(this Enum _this, object obj) => !_this.Equals(obj);

    public static bool EitherWayAround<T>(T jumplane, T whosethebest, T tiedaround, T wiredinsidemi) where T : Enum, IComparable
        => (jumplane.Equals(tiedaround) && whosethebest.Equals(wiredinsidemi)) || (whosethebest.Equals(tiedaround) && jumplane.Equals(wiredinsidemi));

    public static bool OneOrOther<T>(this T _this, T give, T me) where T : Enum, IComparable
        => _this.DoesNotEqual(give) || _this.DoesNotEqual(me);
}
