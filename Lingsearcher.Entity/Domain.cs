using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace Lingseacher.Entity
{
    public abstract class Domain
    {
        public int Id { get; set; }

        public Dictionary<string, object> GetProperties(bool includeInherited = true, bool includeNull = false, bool includeDefault = false, string[] ignore = null, string[] only = null)
        {
            var parameters = new Dictionary<string, object>();

            var bindingFlags = includeInherited ? BindingFlags.Instance | BindingFlags.Public : BindingFlags.Public;

            foreach (var prop in GetType().GetProperties(bindingFlags))
            {
                try
                {
                    if (prop.GetCustomAttribute<NotMappedAttribute>() != null)
                    {
                        continue;
                    }

                    var valor = prop.GetValue(this);

                    if (valor == null && !includeNull)
                    {
                        continue;
                    }

                    if (valor is string)
                    {
                        parameters.Add(prop.Name, valor);
                        continue;
                    }

                    if (valor is Domain || valor is UserApplication)
                    {
                        var id = Convert.ToString(valor.GetType().GetProperty("Id").GetValue(valor));

                        var nome = $"{prop.Name}Id";

                        if (int.TryParse(id, out int idNumber))
                        {
                            if (idNumber != 0 && !parameters.ContainsKey(nome))
                            {
                                parameters.Add(nome, id);
                            }
                        }
                        else
                        {
                            if (id != "0" && !parameters.ContainsKey(nome))
                            {
                                parameters.Add(nome, id);
                            }
                        }


                        continue;
                    }

                    if (!prop.PropertyType.IsValueType)
                    {
                        continue;
                    }

                    if (!includeDefault)
                    {
                        // Verifica se é o valor padrão daquele tipo
                        if (prop.PropertyType != typeof(bool) &&
                           (prop.PropertyType == typeof(int) && prop.Name.Contains("Id") ||
                            prop.PropertyType == typeof(long) && prop.Name.Contains("Id")) &&
                            Equals(valor, Activator.CreateInstance(prop.PropertyType)))
                        {
                            continue;
                        }
                    }

                    parameters.Add(prop.Name, valor);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            if (ignore != null)
            {
                parameters = parameters.Where(p => !ignore.Contains(p.Key)).ToDictionary(x => x.Key, x => x.Value);
            }

            if (only != null)
            {
                parameters = parameters.Where(p => only.Contains(p.Key)).ToDictionary(x => x.Key, x => x.Value);
            }

            return parameters;
        }
    }
}

