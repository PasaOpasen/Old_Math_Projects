using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Qocr.Core.Data.Attributes;
using Qocr.Core.Utils;

namespace Qocr.Core.Data.Map2D
{
    /// <summary>
    /// Эйлеровая характеристика для изображения.
    /// </summary>
    public class EulerMonomap2D
    {
        private static EulerMonomap2D _empty;

        /// <summary>
        /// Пустая эйлеровая характеристика.
        /// </summary>
        public static EulerMonomap2D Empty => _empty ?? (_empty = new EulerMonomap2D());

        private int? _hashCode;

        private const string PropertyPrefix = "S";
        
        /// <summary>
        /// Создание экземпляра класса <see cref="EulerMonomap2D"/>.
        /// </summary>
        public EulerMonomap2D(params int[] characteristics)
        {
            ClassUtils.SetIndexValues(this, PropertyPrefix, characteristics.Cast<object>().ToArray());
        }

        /// <summary>
        /// Создание экземпляра класса <see cref="EulerMonomap2D"/>.
        /// </summary>
        public EulerMonomap2D(string characteristics, char splitter = ',')
        {
            var chars = characteristics.Split(splitter).Select(int.Parse).ToArray();
            if (chars.Length != 15)
            {
                ClassUtils.SetIndexValues(this, PropertyPrefix, chars.Cast<object>().ToArray());
            }
            else
            {
                S0 = chars[0];
                S1 = chars[1];
                S2 = chars[2];
                S3 = chars[3];
                S4 = chars[4];
                S5 = chars[5];
                S6 = chars[6];
                S7 = chars[7];
                S8 = chars[8];
                S9 = chars[9];
                S10 = chars[10];
                S11 = chars[11];
                S12 = chars[12];
                S13 = chars[13];
                S14 = chars[14];
            }
        }

        /// <summary>
        /// Создание экземпляра класса <see cref="EulerMonomap2D"/>.
        /// </summary>
        public EulerMonomap2D(IDictionary<string, int> squares)
        {
            var eulerProperties = GetEulerProperties()
                .Where(property => property.GetCustomAttributes(typeof(EulerPathAttribute), false).Any())
                .ToDictionary(
                    property => property,
                    property =>
                        (EulerPathAttribute)property.GetCustomAttributes(typeof(EulerPathAttribute), false).FirstOrDefault());

            foreach (var square in squares)
            {
                var targetProperty = eulerProperties.First(property => property.Value.Path == square.Key);
                targetProperty.Key.SetValue(this, squares[square.Key], null);
            }
        }

        /// <summary>
        /// <para>[ ] [X]</para> 
        /// <para>[ ] [ ]</para> 
        /// </summary>
        [EulerPath("0100")]
        public int S0 { get; private set; }

        /// <summary>
        /// <para>[ ] [ ]</para> 
        /// <para>[ ] [X]</para> 
        /// </summary>
        [EulerPath("0001")]
        public int S1 { get; private set; }

        /// <summary>
        /// <para>[X] [ ]</para> 
        /// <para>[ ] [ ]</para> 
        /// </summary>
        [EulerPath("1000")]
        public int S2 { get; private set; }

        /// <summary>
        /// <para>[ ] [ ]</para> 
        /// <para>[X] [ ]</para> 
        /// </summary>
        [EulerPath("0010")]
        public int S3 { get; private set; }

        /// <summary>
        /// <para>[X] [X]</para> 
        /// <para>[ ] [ ]</para> 
        /// </summary>
        [EulerPath("1100")]
        public int S4 { get; private set; }

        /// <summary>
        /// <para>[ ] [X]</para> 
        /// <para>[ ] [X]</para> 
        /// </summary>
        [EulerPath("0101")]
        public int S5 { get; private set; }

        /// <summary>
        /// <para>[ ] [ ]</para> 
        /// <para>[X] [X]</para> 
        /// </summary>
        [EulerPath("0011")]
        public int S6 { get; private set; }

        /// <summary>
        /// <para>[X] [ ]</para> 
        /// <para>[X] [ ]</para> 
        /// </summary>
        [EulerPath("1010")]
        public int S7 { get; private set; }

        /// <summary>
        /// <para>[ ] [X]</para> 
        /// <para>[X] [ ]</para> 
        /// </summary>
        [EulerPath("0110")]
        public int S8 { get; private set; }

        /// <summary>
        /// <para>[X] [ ]</para> 
        /// <para>[ ] [X]</para> 
        /// </summary>
        [EulerPath("1001")]
        public int S9 { get; private set; }

        /// <summary>
        /// <para>[X] [X]</para> 
        /// <para>[ ] [X]</para> 
        /// </summary>
        [EulerPath("1101")]
        public int S10 { get; private set; }

        /// <summary>
        /// <para>[X] [ ]</para> 
        /// <para>[X] [X]</para> 
        /// </summary>
        [EulerPath("1011")]
        public int S11 { get; private set; }

        /// <summary>
        /// <para>[ ] [X]</para> 
        /// <para>[X] [X]</para> 
        /// </summary>
        [EulerPath("0111")]
        public int S12 { get; private set; }

        /// <summary>
        /// <para>[X] [X]</para> 
        /// <para>[X] [ ]</para> 
        /// </summary>
        [EulerPath("1110")]
        public int S13 { get; private set; }

        /// <summary>
        /// <para>[X] [X]</para> 
        /// <para>[X] [X]</para> 
        /// </summary>
        [EulerPath("1111")]
        public int S14 { get; private set; }

        /// <inheritdoc/>
        public int SquareSize => 2;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            if (!_hashCode.HasValue)
            {
                int hashCode = 13;
                var allValues = new List<int> { S0, S1, S2, S3, S4, S5, S6, S7, S8, S9, S10, S11, S12, S13, S14 };
                for (int i = 0; i < allValues.Count; i ++)
                {
                    var value = allValues[i];
                    hashCode = hashCode * 13 + value.GetHashCode();
                }

                _hashCode = hashCode;
            }

            return _hashCode.Value;
        }
        
        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            EulerMonomap2D objEuler = obj as EulerMonomap2D;
            if (objEuler == null)
            {
                return false;
            }

            return 
                objEuler.S0  == S0  && 
                objEuler.S1  == S1  &&
                objEuler.S2  == S2  &&
                objEuler.S3  == S3  &&
                objEuler.S4  == S4  &&
                objEuler.S5  == S5  &&
                objEuler.S6  == S6  &&
                objEuler.S7  == S7  &&
                objEuler.S8  == S8  &&
                objEuler.S9  == S9  &&
                objEuler.S10 == S10 &&
                objEuler.S11 == S11 &&
                objEuler.S12 == S12 &&
                objEuler.S13 == S13 &&
                objEuler.S14 == S14;
        }

        /// <summary>
        /// Перегрузка оператора вычисления разницы.
        /// </summary>
        /// <param name="left">Из чего вычитается.</param>
        /// <param name="right">Что вычитается.</param>
        /// <returns>Результат разницы.</returns>
        public static EulerMonomap2D operator -(EulerMonomap2D left, EulerMonomap2D right)
        {
            // TODO Возможно стоит Abs делать так как плюс это разница в одну сторону а минус в другую
            var diff = new[]
            {
                left.S0 - right.S0,
                left.S1 - right.S1,
                left.S2 - right.S2,
                left.S3 - right.S3,
                left.S4 - right.S4,
                left.S5 - right.S5,
                left.S6 - right.S6,
                left.S7 - right.S7,
                left.S8 - right.S8,
                left.S9 - right.S9,
                left.S10 - right.S10,
                left.S11 - right.S11,
                left.S12 - right.S12,
                left.S13 - right.S13,
                left.S14 - right.S14,
            };

            return new EulerMonomap2D(diff);
        }

        /// <summary>
        /// Перегрузка оператора вычисления разницы.
        /// </summary>
        /// <param name="left">Из чего вычитается.</param>
        /// <param name="right">Что вычитается.</param>
        /// <returns>Результат разницы.</returns>
        public static EulerMonomap2D operator +(EulerMonomap2D left, EulerMonomap2D right)
        {
            // TODO Возможно стоит Abs делать так как плюс это разница в одну сторону а минус в другую
            var diff = new[]
            {
                left.S0 + right.S0,
                left.S1 + right.S1,
                left.S2 + right.S2,
                left.S3 + right.S3,
                left.S4 + right.S4,
                left.S5 + right.S5,
                left.S6 + right.S6,
                left.S7 + right.S7,
                left.S8 + right.S8,
                left.S9 + right.S9,
                left.S10 + right.S10,
                left.S11 + right.S11,
                left.S12 + right.S12,
                left.S13 + right.S13,
                left.S14 + right.S14,
            };

            return new EulerMonomap2D(diff);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return string.Join(",", S0, S1, S2, S3, S4, S5, S6, S7, S8, S9, S10, S11, S12, S13, S14);
        }

        private IEnumerable<PropertyInfo> GetEulerProperties()
        {
            return
                GetType()
                    .GetProperties()
                    .Where(property => property.Name.StartsWith(PropertyPrefix))
                    .ToArray();
        }              
    }
}









