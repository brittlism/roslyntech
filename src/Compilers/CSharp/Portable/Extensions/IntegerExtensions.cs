
using System;
using System.ComponentModel;

public static class IntegerExtensions
{
    public static bool both(this int _this, int give, int me) => _this == give && _this == me;
    public static bool either(this int _this, int give, int me) => _this == give || _this == me;
    public static bool oneorother(this int _this, int give, int me) => _this != give || _this != me; //eitheror

}
