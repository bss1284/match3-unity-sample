using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BSS.Extension {
    public static class EnumEx {
        public static T Parse<T>(string value) {
            return (T)Enum.Parse(typeof(T), value);
        }

    }
}
