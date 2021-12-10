using System;
using System.Globalization;

namespace RationalNumbersTask
{
    public sealed class RationalNumbers
    {
        private int _numerator;              // Числитель
        private int _denominator;            // Знаменатель
        private int _sign;                   // Знак
        public RationalNumbers(int numerator, int denominator)
        {
            if (denominator == 0)
            {
                throw new DivideByZeroException("В знаменателе не может быть нуля");
            }
            this._numerator = Math.Abs(numerator);
            this._denominator = Math.Abs(denominator);
            if (numerator * denominator < 0)
            {
                this._sign = -1;
            }
            else
            {
                this._sign = 1;
            }
        }
        // Вызов первого конструктора со знаменателем равным единице
        public RationalNumbers(int number) : this(number, 1) { }

        // Возвращает наибольший общий делитель (Алгоритм Евклида)
        private static int getGreatestCommonDivisor(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        // Возвращает наименьшее общее кратное
        private static int getLeastCommonMultiple(int a, int b)
        {
            // В формуле опушен модуль, так как в классе
            // числитель всегда неотрицательный, а знаменатель -- положительный
            // ...
            // Деление здесь -- челочисленное, что не искажает результат, так как
            // числитель и знаменатель делятся на свои делители,
            // т.е. при делении не будет остатка
            return a * b / getGreatestCommonDivisor(a, b);
        }

        // Возвращает дробь, которая является результатом сложения или вычитания дробей a и b,
        // В зависимости от того, какая операция передана в параметр operation.
        // P.S. использовать только для сложения и вычитания
        private static RationalNumbers performOperation(RationalNumbers a, RationalNumbers b, Func<int, int, int> operation)
        {
            // Наименьшее общее кратное знаменателей
            int leastCommonMultiple = getLeastCommonMultiple(a._denominator, b._denominator);
            // Дополнительный множитель к первой дроби
            int additionalMultiplierFirst = leastCommonMultiple / a._denominator;
            // Дополнительный множитель ко второй дроби
            int additionalMultiplierSecond = leastCommonMultiple / b._denominator;
            // Результат операции
            int operationResult = operation(a._numerator * additionalMultiplierFirst * a._sign,
            b._numerator * additionalMultiplierSecond * b._sign);
            return new RationalNumbers(operationResult, a._denominator * additionalMultiplierFirst);
        }

        // Перегрузка оператора "+" для случая сложения двух дробей
        public static RationalNumbers operator +(RationalNumbers a, RationalNumbers b)
        {
            return performOperation(a, b, (int x, int y) => x + y);
        }
        // Перегрузка оператора "+" для случая сложения дроби с числом
        public static RationalNumbers operator +(RationalNumbers a, int b)
        {
            return a + new RationalNumbers(b);
        }
        // Перегрузка оператора "+" для случая сложения числа с дробью
        public static RationalNumbers operator +(int a, RationalNumbers b)
        {
            return b + a;
        }
        // Перегрузка оператора "-" для случая вычитания двух дробей
        public static RationalNumbers operator -(RationalNumbers a, RationalNumbers b)
        {
            return performOperation(a, b, (int x, int y) => x - y);
        }
        // Перегрузка оператора "-" для случая вычитания из дроби числа
        public static RationalNumbers operator -(RationalNumbers a, int b)
        {
            return a - new RationalNumbers(b);
        }
        // Перегрузка оператора "-" для случая вычитания из числа дроби
        public static RationalNumbers operator -(int a, RationalNumbers b)
        {
            return b - a;
        }
        // Перегрузка оператора "*" для случая произведения двух дробей
        public static RationalNumbers operator *(RationalNumbers a, RationalNumbers b)
        {
            return new RationalNumbers(a._numerator * a._sign * b._numerator * b._sign, a._denominator * b._denominator);
        }
        // Перегрузка оператора "*" для случая произведения дроби и числа
        public static RationalNumbers operator *(RationalNumbers a, int b)
        {
            return a * new RationalNumbers(b);
        }
        // Перегрузка оператора "*" для случая произведения числа и дроби
        public static RationalNumbers operator *(int a, RationalNumbers b)
        {
            return b * a;
        }
        // Перегрузка оператора "/" для случая деления двух дробей
        public static RationalNumbers operator /(RationalNumbers a, RationalNumbers b)
        {
            return a * b.GetReverse();
        }
        // Перегрузка оператора "/" для случая деления дроби на число
        public static RationalNumbers operator /(RationalNumbers a, int b)
        {
            return a / new RationalNumbers(b);
        }
        // Перегрузка оператора "/" для случая деления числа на дробь
        public static RationalNumbers operator /(int a, RationalNumbers b)
        {
            return new RationalNumbers(a) / b;
        }
        // Перегрузка оператора "унарный минус"
        public static RationalNumbers operator -(RationalNumbers a)
        {
            return a.GetWithChangedSign();
        }
        // Перегрузка оператора "++"
        public static RationalNumbers operator ++(RationalNumbers a)
        {
            return a + 1;
        }
        // Перегрузка оператора "--"
        public static RationalNumbers operator --(RationalNumbers a)
        {
            return a - 1;
        }

        // Возвращает дробь, обратную данной
        private RationalNumbers GetReverse()
        {
            return new RationalNumbers(this._denominator * this._sign, this._numerator);
        }
        // Возвращает дробь с противоположным знаком
        private RationalNumbers GetWithChangedSign()
        {
            return new RationalNumbers(-this._numerator * this._sign, this._denominator);
        }

        // Мой метод Equals
        public bool Equals(RationalNumbers that)
        {
            RationalNumbers a = this.Reduce();
            RationalNumbers b = that.Reduce();
            return a._numerator == b._numerator &&
            a._denominator == b._denominator &&
            a._sign == b._sign;
        }
        // Переопределение метода Equals
        public override bool Equals(object obj)
        {
            bool result = false;
            if (obj is RationalNumbers)
            {
                result = this.Equals(obj as RationalNumbers);
            }
            return result;
        }
        // Переопределение метода GetHashCode
        public override int GetHashCode()
        {
            return this._sign * (this._numerator * this._numerator + this._denominator * this._denominator);
        }

        // Перегрузка оператора "Равенство" для двух дробей
        public static bool operator ==(RationalNumbers a, RationalNumbers b)
        {
            // Приведение к Object необходимо для того, чтобы
            // можно было сравнивать дроби с null.
            // Обычное сравнение a.Equals(b) в данном случае не подходит,
            // так как если a есть null, то у него нет метода Equals,
            // следовательно будет выдано исключение, а если
            // b окажется равным null, то исключение будет вызвано в
            // методе this.Equals
            Object aAsObj = a as Object;
            Object bAsObj = b as Object;
            if (aAsObj == null || bAsObj == null)
            {
                return aAsObj == bAsObj;
            }
            return a.Equals(b);
        }
        // Перегрузка оператора "Равенство" для дроби и числа
        public static bool operator ==(RationalNumbers a, int b)
        {
            return a == new RationalNumbers(b);
        }
        // Перегрузка оператора "Равенство" для числа и дроби
        public static bool operator ==(int a, RationalNumbers b)
        {
            return new RationalNumbers(a) == b;
        }
        // Перегрузка оператора "Неравенство" для двух дробей
        public static bool operator !=(RationalNumbers a, RationalNumbers b)
        {
            return !(a == b);
        }
        // Перегрузка оператора "Неравенство" для дроби и числа
        public static bool operator !=(RationalNumbers a, int b)
        {
            return a != new RationalNumbers(b);
        }
        // Перегрузка оператора "Неравенство" для числа и дроби
        public static bool operator !=(int a, RationalNumbers b)
        {
            return new RationalNumbers(a) != b;
        }

        // Метод сравнения двух дробей
        // Возвращает	 0, если дроби равны
        //				 1, если this больше that
        //				-1, если this меньше that
        private int CompareTo(RationalNumbers that)
        {
            if (this.Equals(that))
            {
                return 0;
            }
            RationalNumbers a = this.Reduce();
            RationalNumbers b = that.Reduce();
            if (a._numerator * a._sign * b._denominator > b._numerator * b._sign * a._denominator)
            {
                return 1;
            }
            return -1;
        }

        // Перегрузка оператора ">" для двух дробей
        public static bool operator >(RationalNumbers a, RationalNumbers b)
        {
            return a.CompareTo(b) > 0;
        }
        // Перегрузка оператора ">" для дроби и числа
        public static bool operator >(RationalNumbers a, int b)
        {
            return a > new RationalNumbers(b);
        }
        // Перегрузка оператора ">" для числа и дроби
        public static bool operator >(int a, RationalNumbers b)
        {
            return new RationalNumbers(a) > b;
        }
        // Перегрузка оператора "<" для двух дробей
        public static bool operator <(RationalNumbers a, RationalNumbers b)
        {
            return a.CompareTo(b) < 0;
        }
        // Перегрузка оператора "<" для дроби и числа
        public static bool operator <(RationalNumbers a, int b)
        {
            return a < new RationalNumbers(b);
        }
        // Перегрузка оператора "<" для числа и дроби
        public static bool operator <(int a, RationalNumbers b)
        {
            return new RationalNumbers(a) < b;
        }
        // Перегрузка оператора ">=" для двух дробей
        public static bool operator >=(RationalNumbers a, RationalNumbers b)
        {
            return a.CompareTo(b) >= 0;
        }
        // Перегрузка оператора ">=" для дроби и числа
        public static bool operator >=(RationalNumbers a, int b)
        {
            return a >= new RationalNumbers(b);
        }
        // Перегрузка оператора ">=" для числа и дроби
        public static bool operator >=(int a, RationalNumbers b)
        {
            return new RationalNumbers(a) >= b;
        }
        // Перегрузка оператора "<=" для двух дробей
        public static bool operator <=(RationalNumbers a, RationalNumbers b)
        {
            return a.CompareTo(b) <= 0;
        }
        // Перегрузка оператора "<=" для дроби и числа
        public static bool operator <=(RationalNumbers a, int b)
        {
            return a <= new RationalNumbers(b);
        }
        // Перегрузка оператора "<=" для числа и дроби
        public static bool operator <=(int a, RationalNumbers b)
        {
            return new RationalNumbers(a) <= b;
        }

        // Возвращает сокращенную дробь
        public RationalNumbers Reduce()
        {
            RationalNumbers result = this;
            int greatestCommonDivisor = getGreatestCommonDivisor(this._numerator, this._denominator);
            result._numerator /= greatestCommonDivisor;
            result._denominator /= greatestCommonDivisor;
            return result;
        }
        // Переопределение метода ToString
        public override string ToString()
        {
            if (this._numerator == 0)
            {
                return "0";
            }
            string result;
            if (this._sign < 0)
            {
                result = "-";
            }
            else
            {
                result = "";
            }
            if (this._numerator == this._denominator)
            {
                return result + "1";
            }
            if (this._denominator == 1)
            {
                return result + this._numerator;
            }
            return result + this._numerator + "/" + this._denominator;
        }
        //оператор преобразования типов из типа с плавающей точкой в тип дроби
        public static implicit operator RationalNumbers(double numeric)
        {
            //Разбиваем число на целую и дробную часть
            var numericArray = numeric.ToString().Split(new[] { CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator }, StringSplitOptions.None);
            var wholeStr = numericArray[0];
            var fractionStr = "0";
            if (numericArray.Length > 1)
                fractionStr = numericArray[1];

            //Получаем степень десятки, на которую нужно умножить число, чтобы дробь стала целым 
            var power = fractionStr.Length;

            //Получаем целую часть числителя и знаменатель
            long whole = long.Parse(wholeStr) * 10;
            long denominator = 10;
            for (int i = 1; i < power; i++)
            {
                denominator = denominator * 10;
                whole = whole * 10;
            }

            //получаем числитель
            var numerator = long.Parse(fractionStr);
            numerator = numerator + whole;
            //конвертим long в int
            int numerator1 = Convert.ToInt32(numerator);
            int denominator1 = Convert.ToInt32(denominator);

            //Ищем общий знаменатель и делим на него

            int greatestCommonDivisor = getGreatestCommonDivisor(numerator1, denominator1);
            numerator1 /= greatestCommonDivisor;
            denominator1 /= greatestCommonDivisor;


            return new RationalNumbers(numerator1, denominator1);
        }

        //оператор преобразования типов из типа дроби в тип с плавающей точкой

        public static explicit operator double(RationalNumbers numeric)
        {
            var result = (double)numeric._numerator / numeric._denominator;
            return (double)result;
        }
    }

            // Пример проверки
    class Program
    {
        static void Main(string[] args)
        {
            RationalNumbers fraction = new RationalNumbers(4, 8);
            RationalNumbers fraction1 = new RationalNumbers(4, 2);

            if (fraction >= fraction1)
            {
                Console.WriteLine("true");
            }
            else
                Console.WriteLine("false");

            Console.WriteLine(fraction1.ToString());

            double a = 11.154;
            RationalNumbers lel = a;

            Console.WriteLine(lel);

            double d = (double)fraction;

            Console.WriteLine(d);
        }
    }
}