using DBTestStresser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DBTestStresser.Util {
    public class RandomDB {
        string[] TABLES_NAMES = {
            "customers","brands","orders","products"
        };

        private static readonly Random rng = new Random();
        public static string[] GenerateRandomSQLReadQueries(int amount) {
            var queries = new string[amount];
            int startIndex = RandomDB.GenerateRandomInt(1, EntityDBMS.N_PRODUCTS-100);
            int productNbPerPage = 100;
            for (int i = 0; i < amount; i++) {
                queries[i] =
                    String.Format(
                    "SELECT * FROM products " +
                    "JOIN brands on products.brand_id = brands.id " +
                    "WHERE products.id BETWEEN {0} AND {1};",
                    startIndex, startIndex + productNbPerPage
                );
            }
            return queries;
        }

        public static string[] GenerateRandomMongoSHFilters(int amount) {
            int productNbPerPage = 100;
            string[] filters = new string[amount];
            int greater, lower;
            string filter;
            for (int i = 0; i < amount; i++) {
                greater = RandomDB.GenerateRandomInt(0, EntityDBMS.N_PRODUCTS - productNbPerPage);
                lower = greater + productNbPerPage;
                filter =
                    "{" +
                        "_id : {" +
                            "$gte:" + greater + "," +
                            "$lte: " + lower + "" +
                        "}" +
                    "}";
                filters[i] = filter;
            }
            

            return filters;
        }

        public static string[] GenerateRandomSQLUpdateQueries(int amount) {
            
            string[] queries = new string[amount];
            for (int i = 0; i < amount; i++) {
                queries[i] = "UPDATE products SET stock=stock+1 WHERE id="
                    + GenerateRandomInt(0, EntityDBMS.N_PRODUCTS);

            }

            return queries;
            
        }

        public static string[] GenerateRandomMongoJsons(int amount) {
            string[] jsons = new string[amount];
            
            for (int i = 0; i < amount; i++) {
                
                jsons[i] = Order.GenerateRandom(EntityDBMS.N_PRODUCTS, EntityDBMS.N_CUSTOMERS,
                    EntityDBMS.N_BRANDS, -1).ToJsonString();
            }

            return jsons;
        }
        public static string[] GenerateRandomSQLWriteQueries(int amount) {
            var queries = new string[amount];
            
            for (int i = 0; i < amount; i++) {
                queries[i] = String.Format("INSERT INTO orders(date,customer_id,product_id) " +
                    "VALUES ('{0}',{1},{2})",
                    RandomDB.GenerateRandomString(10),
                    RandomDB.GenerateRandomInt(1, EntityDBMS.N_CUSTOMERS),
                    RandomDB.GenerateRandomInt(1, EntityDBMS.N_PRODUCTS)
                    );
            }
            return queries;
        }

        public static string BuildInsertIntoValues(List<string> types, int amount,
            int[] intMins = null,
            int[] intMaxs = null,
            int stringLength = 10) {

            string values = " VALUES (";
            

            // Default values
            if (intMins == null) {
                intMins = new int[] { 0};
            }
            if (intMaxs == null) {
                intMaxs = new int[] { 50 };
            }

            if (types.Count == 0 || intMaxs.Length != intMins.Length) {
                return null;
            }
            int indexBoundaries;
            for (int i = 0; i < amount; i++) {
                indexBoundaries = 0;
                foreach (string t in types) {
                    switch (t) {
                        case "int":
                            values += GenerateRandomInt(intMins[indexBoundaries], 
                                intMaxs[indexBoundaries]) + ",";
                            indexBoundaries++;
                            break;
                        case "double":
                            values += (double)GenerateRandomInt(intMins[indexBoundaries],
                                intMaxs[indexBoundaries]) + ",";
                            indexBoundaries++;
                            break;
                        case "string":
                            values += "'" + GenerateRandomString(stringLength) + "',";
                            break;
                        default:
                            break;
                    }
                    
                }
                // Remove last ',' and close parenthesis
                values = values.Substring(0, values.Length - 1) + ")";
                if (i < amount - 1) {
                    values += ",(";
                }
            }


            return values;
        }

        public static List<string> BuildCQLInsertIntoValues(
            List<string> types, 
            string insertRoot, 
            int amount,
            int[] intMins = null,
            int[] intMaxs = null,
            int stringLength = 10) {

            string cql;
            List<string> cqls = new List<string>();

            // Default values
            if (intMins == null) {
                intMins = new int[] { 0 };
            }
            if (intMaxs == null) {
                intMaxs = new int[] { 50 };
            }

            if (types.Count == 0 || intMaxs.Length != intMins.Length) {
                return null;
            }
            int indexBoundaries;
            for (int i = 0; i < amount; i++) {
                indexBoundaries = 0;
                cql = insertRoot + " VALUES (";
                foreach (string t in types) {
                    switch (t) {
                        case "int":
                            cql += GenerateRandomInt(intMins[indexBoundaries],
                                intMaxs[indexBoundaries]) + ",";
                            indexBoundaries++;
                            break;
                        case "double":
                            cql += (double) GenerateRandomInt(intMins[indexBoundaries],
                                intMaxs[indexBoundaries]) + ",";
                            indexBoundaries++;
                            break;
                        case "string":
                            cql += "'" + GenerateRandomString(stringLength) + "',";
                            break;
                        default:
                            break;
                    }

                }
                // Remove last ',' and close parenthesis
                cql = cql.Substring(0, cql.Length - 1) + ");";
                cqls.Add(cql);
            }


            return cqls;
        }
        public static List<string> BuildInsertThousandSplitted(string insert, List<string> types, int amount,
            int[] intMins = null,int[] intMaxs = null,int stringLength = 10) {

                int split = 999;
                string basis = insert + " VALUES (";
                string values = basis;
                var inserts = new List<string>();

                // Default values
                if (intMins == null) {
                    intMins = new int[] { 0 };
                }
                if (intMaxs == null) {
                    intMaxs = new int[] { 50 };
                }

                if (types.Count == 0 || intMaxs.Length != intMins.Length) {
                    return null;
                }

                int indexBoundaries;
                for (int i = 0; i < amount; i++) {
                    indexBoundaries = 0;
                    foreach (string t in types) {
                        switch (t) {
                            case "int":
                                values += GenerateRandomInt(intMins[indexBoundaries],
                                    intMaxs[indexBoundaries]) + ",";
                                indexBoundaries++;
                                break;
                            case "double":
                                values += (double) GenerateRandomInt(intMins[indexBoundaries],
                                    intMaxs[indexBoundaries]) + ",";
                                indexBoundaries++;
                                break;
                            case "string":
                                values += "'" + GenerateRandomString(stringLength) + "',";
                                break;
                            default:
                                break;
                        }

                    }

                    // Remove last ',' and close parenthesis
                    values = values.Substring(0, values.Length - 1) + ")";
                    if (i != 0 && i % split == 0) { // Split following requests
                        values += ';';
                        inserts.Add(values);
                        
                        values = i < amount - 1 ? basis : "";
                    } else if (i < amount - 1) {
                        values += ",(";
                    }
                }

                if (!String.IsNullOrEmpty(values))
                    inserts.Add(values);

                return inserts;
            }
        public static string GenerateRandomString(int length) {
            
            string r = "";
            const string CHARS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            int chars_size = CHARS.Length;
            for (int i = 0; i < length; i++) {
                r += CHARS[rng.Next(0, chars_size)];
            }

            return r;
        }

        public static int GenerateRandomInt(int min, int max) {
            
            rng.Next();
            return rng.Next(min, max + 1);
        }
    }
}
