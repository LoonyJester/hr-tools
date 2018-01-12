using System;
using System.Collections.Generic;
using System.Linq;

namespace HRTools.Crosscutting.Common
{
    public static class Guard
    {
        #region Argument

        public static void ArgumentIsNotNull(object argument, string name)
        {
            if (argument == null)
            {
                throw new ArgumentException(ExceptionMessages.ArgumentExceptionNull, name);
            }
        }

        public static void ArgumentIsNotNullOrEmpty(string argument, string name)
        {
            if (string.IsNullOrEmpty(argument))
            {
                throw new ArgumentException(ExceptionMessages.ArgumentExceptionNullOrEmpty, name);
            }
        }

        public static void ArgumentIsNotNullOrEmpty(Guid argument, string name)
        {
            if (argument == Guid.Empty)
            {
                throw new ArgumentException(ExceptionMessages.ArgumentExceptionNullOrEmpty, name);
            }
        }

        public static void ArgumentIsNotNullOrWhiteSpace(string argument, string name)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                throw new ArgumentException(ExceptionMessages.ArgumentExceptionNullOrEmpty, name);
            }
        }
        
        public static void ArgumentIsNullOrNotEmpty(string argument, string name)
        {
            if (argument != null && argument.Length == 0)
            {
                throw new ArgumentException(ExceptionMessages.ArgumentIsNullOrNotEmpty, name);
            }
        }

        public static void ArgumentIsNotNullOrEmpty<T>(IEnumerable<T> argument, string name)
        {
            if (argument == null || !argument.Any())
            {
                throw new ArgumentException(ExceptionMessages.ArgumentExceptionNullOrEmpty, name);
            }
        }

        public static void ArgumentIsNotNullOrEmpty<T>(ICollection<T> argument, string name)
        {
            if (argument == null || argument.Count == 0)
            {
                throw new ArgumentException(ExceptionMessages.ArgumentExceptionNullOrEmpty, name);
            }
        }

        public static void ArgumentIsNotNullOrEmtpy<T>(IReadOnlyCollection<T> argument, string name)
        {
            if (argument == null || argument.Count == 0)
            {
                throw new ArgumentException(ExceptionMessages.ArgumentExceptionNullOrEmpty, name);
            }
        }

        public static void ArgumentIsTrue(bool argument, string name)
        {
            if (!argument)
            {
                throw new ArgumentException(ExceptionMessages.ArgumentExceptionFalseWhenTrueExpected, name);
            }
        }

        public static void ArgumentIsFalse(bool argument, string name)
        {
            if (argument)
            {
                throw new ArgumentException(ExceptionMessages.ArgumentExceptionTrueWhenFalseExpected, name);
            }
        }

        public static void ArgumentIsGreaterThanZero(int argument, string name)
        {
            if (argument <= 0)
            {
                throw new ArgumentException(ExceptionMessages.ArgumentExceptionGreaterThanZero, name);
            }
        }

        public static void ArgumentIsGreaterThanZero(decimal argument, string name)
        {
            if (argument <= 0)
            {
                throw new ArgumentException(ExceptionMessages.ArgumentExceptionGreaterThanZero, name);
            }
        }

        public static void ArgumentIsGreaterOrEqualZero(decimal argument, string name)
        {
            if (argument < 0)
            {
                throw new ArgumentException(ExceptionMessages.ArgumentExceptionGreaterOrEqualZero, name);
            }
        }

        public static void ArgumentIsNullOrGreaterThanZero(int? argument, string name)
        {
            if (argument <= 0)
            {
                throw new ArgumentException(ExceptionMessages.ArgumentExceptionGreaterThanZero, name);
            }
        }

        public static void ArgumentIsNullOrGreaterOrEqualZero(int? argument, string name)
        {
            if (argument < 0)
            {
                throw new ArgumentException(ExceptionMessages.ArgumentExceptionGreaterThanZero, name);
            }
        }

        public static void ArgumentIsNullOrGreaterOrEqualZero(decimal? argument, string name)
        {
            if (argument < 0)
            {
                throw new ArgumentException(ExceptionMessages.ArgumentExceptionGreaterThanZero, name);
            }
        }

        public static void ArgumentIsGreaterThan(int argument, int limit, string name)
        {
            if (argument <= limit)
            {
                throw new ArgumentException(string.Format(ExceptionMessages.ArgumentExceptionGreaterThan, limit), name);
            }
        }

        public static void ArgumentIsGreaterThan(uint argument, uint limit, string name)
        {
            if (argument <= limit)
            {
                throw new ArgumentException(string.Format(ExceptionMessages.ArgumentExceptionGreaterThan, limit), name);
            }
        }

        public static void ArgumentIsGreaterThan(DateTime argument, DateTime limit, string name)
        {
            if (argument <= limit)
            {
                throw new ArgumentException(string.Format(ExceptionMessages.ArgumentExceptionGreaterThan, limit), name);
            }
        }

        public static void ArgumentIsGreaterOrEqual(int argument, int limit, string name)
        {
            if (argument < limit)
            {
                throw new ArgumentException(string.Format(ExceptionMessages.ArgumentExceptionGreaterOrEqual, limit), name);
            }
        }

        public static void ArgumentIsGreaterOrEqual(uint argument, int limit, string name)
        {
            if (argument < limit)
            {
                throw new ArgumentException(string.Format(ExceptionMessages.ArgumentExceptionGreaterOrEqual, limit), name);
            }
        }

        public static void ArgumentIsGreaterOrEqual(DateTime? argument, DateTime dateToCompare, string name)
        {
            if (!argument.HasValue)
            {
                throw new ArgumentException(ExceptionMessages.ArgumentExceptionNull, name);
            }

            if (argument.Value.Date < dateToCompare.Date)
            {
                throw new ArgumentException(string.Format(ExceptionMessages.ArgumentExceptionGreaterOrEqual, dateToCompare.ToShortDateString()), name);
            }
        }

        public static void ArgumentIsGreaterOrEqual(decimal argument, decimal limit, string name)
        {
            if (argument < limit)
            {
                throw new ArgumentException(string.Format(ExceptionMessages.ArgumentExceptionGreaterOrEqual, limit), name);
            }
        }
        
        public static void ArgumentIsEqualZero(decimal argument, string name)
        {
            if (argument != 0)
            {
                throw new ArgumentException(ExceptionMessages.ArgumentExceptionNotEqualsZero, name);
            }
        }

        public static void ArgumentIsNotEqualTo(decimal argument, decimal valueToAvoid, string name)
        {
            if (argument == valueToAvoid)
            {
                throw new ArgumentException(ExceptionMessages.ArgumentExceptionValueShouldNotBeEqualTo, name);
            }
        }

        /// <summary>
        /// Method to ensure that provided param is greated than current time in UTC 
        /// </summary>
        /// <param name="argument">datetime in utc(!) format</param>
        /// <param name="name">name pf the argument</param>
        public static void ArgumentIsGreaterThanUtcNow(DateTime? argument, string name)
        {
            DateTime utcNowSQL = DateTime.UtcNow;
            if (argument <= utcNowSQL)
            {
                throw new ArgumentException(string.Format(ExceptionMessages.ArgumentExceptionGreaterThan, utcNowSQL), name);
            }
        }

        public static void ArgumentIsValidEnumValue<T>(T argument, string name) where T : struct
        {
            if (!(argument is Enum))
            {
                return;
            }

            if (!Enum.IsDefined(typeof(T), argument))
            {
                throw new ArgumentException(ExceptionMessages.ArgumentExceptionIsNotDefinedInEnum, name);
            }
        }

        public static void ArgumentIsValidEnumValueOrNull<T>(T argument, string name)
        {
            if (argument == null)
            {
                return;
            }

            if (!(argument is Enum))
            {
                return;
            }
            if (!Enum.IsDefined(typeof(T), argument))
            {
                throw new ArgumentException(ExceptionMessages.ArgumentExceptionIsNotDefinedInEnum, name);
            }
        }
        #endregion

        #region Constructor Argument

        public static void ConstructorArgumentIsNotNull(object argument, string name)
        {
            if (argument == null)
            {
                throw new ArgumentException(ExceptionMessages.ConstructorArgumentExceptionNull, name);
            }
        }

        public static void ConstructorArgumentIsNotNullOrEmpty(string argument, string name)
        {
            if (string.IsNullOrEmpty(argument))
            {
                throw new ArgumentException(ExceptionMessages.ConstructorArgumentExceptionNullOrEmpty, name);
            }
        }

        public static void ConstructorArgumentIsNotNullOrEmpty(Guid argument, string name)
        {
            if (argument == null || argument == Guid.Empty)
            {
                throw new ArgumentException(ExceptionMessages.ConstructorArgumentExceptionNullOrEmpty, name);
            }
        }

        public static void ConstructorArgumentIsGreaterThanZero(int argument, string name)
        {
            if (argument <= 0)
            {
                throw new ArgumentException(ExceptionMessages.ConstructorArgumentExceptionGreaterThanZero, name);
            }
        }

        public static void ConstructorArgumentIsGreaterThan(int argument, int limit, string name)
        {
            if (argument <= limit)
            {
                throw new ArgumentException(string.Format(ExceptionMessages.ArgumentExceptionGreaterThan, limit), name);
            }
        }

        public static void ConstructorArgumentIsGreaterThanOrEqual(int argument, int limit, string name)
        {
            if (argument < limit)
            {
                throw new ArgumentException(string.Format(ExceptionMessages.ArgumentExceptionGreaterOrEqual, limit), name);
            }
        }
        #endregion

        public static class Conditional
        {
            public static bool IsArgumentGreaterThanZero(int argument, string name, bool isToThrowExceptionIfFailed = false)
            {
                if (argument <= 0)
                {
                    if (isToThrowExceptionIfFailed)
                    {
                        throw new ArgumentException(ExceptionMessages.ArgumentExceptionGreaterThanZero, name);
                    }
                    return false;
                }
                return true;
            }

            public static bool IsArgumentIsNotNegative(int argument, string name, bool isToThrowExceptionIfFailed = false)
            {
                if (argument < 0)
                {
                    if (isToThrowExceptionIfFailed)
                    {
                        throw new ArgumentException(ExceptionMessages.ArgumentExceptionNotNegativeValue, name);
                    }
                    return false;
                }
                return true;
            }

            public static bool IsArgumentNotNullOrWhiteSpace(string argument, string name, bool isToThrowExceptionIfFailed = false)
            {
                if (string.IsNullOrWhiteSpace(argument))
                {
                    if (isToThrowExceptionIfFailed)
                    {
                        throw new ArgumentException(ExceptionMessages.ArgumentExceptionNullOrEmpty, name);
                    }
                    return false;
                }
                return true;
            }

            public static bool IsArgumentNotNull<T>(T argument, string name = "", bool isToThrowExceptionIfFailed = false) where T : class
            {
                if (argument == null)
                {
                    if (isToThrowExceptionIfFailed)
                    {
                        throw new ArgumentException(ExceptionMessages.ArgumentExceptionNull, name);
                    }
                    return false;
                }
                return true;
            }

            public static bool IsListNotNullOrEmpty<T>(IList<T> items, string name, bool isToThrowExceptionIfFailed = false)
            {
                if (items == null || items.Count == 0)
                {
                    if (isToThrowExceptionIfFailed)
                    {
                        throw new ArgumentException(ExceptionMessages.CollectionIsNullOrEmpty, name);
                    }
                    return false;
                }
                return true;
            }

            public static bool IsEntriesUnique<T>(IEnumerable<T> values)
            {
                HashSet<T> set = new HashSet<T>();

                return values.All(set.Add);
            }
        }

        private static class ExceptionMessages
        {
            internal const string ArgumentExceptionGreaterThanZero = "Argument must be greater than 0";
            internal const string ArgumentExceptionGreaterOrEqualZero = "Argument must be greater or equal 0";
            internal const string ArgumentExceptionNotNegativeValue = "Argument must be equal or greater than 0";
            internal const string ArgumentExceptionGreaterThan = "Argument must be greater than {0}";
            internal const string ArgumentExceptionGreaterOrEqual = "Argument must be greater or equal {0}";
            internal const string ArgumentExceptionNull = "Argument must not be null";
            internal const string ArgumentExceptionNullOrEmpty = "Argument must not be null or empty";
            internal const string ArgumentIsNullOrNotEmpty = "Argument must not be empty";
            internal const string CollectionIsNullOrEmpty = "Collection should be not null and not empty";
            internal const string ArgumentExceptionFalseWhenTrueExpected = "Argument must be set to 'True'";
            internal const string ArgumentExceptionTrueWhenFalseExpected = "Argument must be set to 'False'";
            internal const string ConstructorArgumentExceptionNull = "Constructor argument must not be null";
            internal const string ConstructorArgumentExceptionNullOrEmpty = "Constructor argument must not be null or empty";
            internal const string ConstructorArgumentExceptionGreaterThanZero = "Constructor argument must be greater than zero";
            internal const string ArgumentExceptionNotEqualsZero = "Argument must be equal 0";
            internal const string ArgumentExceptionIsNotDefinedInEnum = "Argument must be defined in enum";
            internal const string ArgumentExceptionValueShouldNotBeEqualTo = "Argument shoud not be equal to '{0}'";
        }
    }
}