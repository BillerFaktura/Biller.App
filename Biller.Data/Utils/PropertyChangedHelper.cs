using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Biller.Data.Utils
{
    public abstract class PropertyChangedHelper : INotifyPropertyChanged
    {
        public PropertyChangedHelper()
        {
            propertyValueStorage = new Dictionary<string, object>();
        }

        #region PropertyHelper

        private Dictionary<string, object> propertyValueStorage;
        /// <summary>
        /// Get the value of the property
        /// </summary>
        /// <typeparam name="T">The property type</typeparam>
        /// <param name="property">The property as a lambda expression</param>
        /// <returns>The value of the given property (or the default value)</returns>
        public T GetValue<T>(Expression<Func<T>> property)
        {
            LambdaExpression lambdaExpression = property as LambdaExpression;

            if (lambdaExpression == null)
            {
                throw new ArgumentException("Invalid lambda expression", "Lambda expression return value can't be null");
            }

            string propertyName = this.getPropertyName(lambdaExpression);

            return getValue<T>(propertyName);
        }

        /// <summary>
        /// Try to get the value from the internal dictionary of the given property name
        /// </summary>
        /// <typeparam name="T">The property type</typeparam>
        /// <param name="propertyName">The name of the property</param>
        /// <returns>Retrieve the value from the internal dictionary</returns>
        private T getValue<T>(string propertyName)
        {
            object value;

            if (propertyValueStorage.TryGetValue(propertyName, out value))
            {
                return (T)value;
            }

            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// Extract the property name from a lambda expression
        /// </summary>
        /// <param name="lambdaExpression">The lambda expression with the property</param>
        /// <returns>The extracted property name</returns>
        private string getPropertyName(LambdaExpression lambdaExpression)
        {
            MemberExpression memberExpression;

            if (lambdaExpression.Body is UnaryExpression)
            {
                var unaryExpression = lambdaExpression.Body as UnaryExpression;

                memberExpression = unaryExpression.Operand as MemberExpression;
            }

            else
            {
                memberExpression = lambdaExpression.Body as MemberExpression;
            }

            return memberExpression.Member.Name;
        }

        /// <summary>
        /// Saves the value of an object in a dictionary and automatically raises a PropertyChangedEvent if the value differs.
        /// </summary>
        /// <typeparam name="T">Your object type</typeparam>
        /// <param name="value">Instance of your object</param>
        /// <param name="name">The name of the calling member.</param>
        public void SetValue<T>(T value, [CallerMemberName] string name = "")
        {
            if (!EqualityComparer<T>.Default.Equals(getValue<T>(name), value))
            {
                propertyValueStorage[name] = value;
                var handler = PropertyChanged;
                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(name));
                }
            }
        }

        public void RaiseUpdateManually([CallerMemberName] string name = "")
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        #endregion
    }
}
