using System;
using System.Collections.Generic;
using System.Text;

namespace StringEnum.Sample.NewtonsoftSerialization
{
    class Animal : StringEnum<Animal>
    {
        public static readonly Animal Cat = New("Cat");
        public static readonly Animal Dog = New("Dog"); 
    }
}
