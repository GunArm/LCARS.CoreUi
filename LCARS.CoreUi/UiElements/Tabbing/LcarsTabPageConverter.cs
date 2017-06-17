using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

namespace LCARS.CoreUi.UiElements.Tabbing
{
    internal class LcarsTabPageConverter : TypeConverter
    {
        //A class to convert LcarsPages during the serialization process.  Don't ask me, I pretty
        //much just changed the word 'button' from the example to 'LcarsPage' and it worked...

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
        {
            if (object.ReferenceEquals(destType, typeof(InstanceDescriptor)))
            {
                return true;
            }

            return base.CanConvertTo(context, destType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destType)
        {
            if (object.ReferenceEquals(destType, typeof(InstanceDescriptor)))
            {
                System.Reflection.ConstructorInfo ci = typeof(LcarsTabPage).GetConstructor(System.Type.EmptyTypes);

                return new InstanceDescriptor(ci, null, false);
            }

            return base.ConvertTo(context, culture, value, destType);
        }
    }
}
