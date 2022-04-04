﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LinqExercises
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            var query = from product in ProductsDatabase.AllProducts
                        join category in ProductsDatabase.AllCategories on product.CategoryId equals category.Id into categoriesGroup
                        from catOrDefault in categoriesGroup.DefaultIfEmpty(new Category(-1, "N/A"))
                        select new
                        {
                            Product = product,
                            Category = catOrDefault
                        };
            */

            var query = ProductsDatabase.AllProducts
                .GroupJoin(
                    ProductsDatabase.AllCategories,
                    product => product.CategoryId,
                    category => category.Id,
                    (product, categories) => new
                    {
                        Product = product,
                        Categories = categories
                    })
                .SelectMany(
                    groupJoin => groupJoin.Categories.DefaultIfEmpty(new Category(-1, "N/A")),
                    (groupJoin, category) => new
                    {
                        Product = groupJoin.Product,
                        Category = category
                    });


            foreach (var pair in query)
            {
                Console.WriteLine($"{pair.Product.Id}) {pair.Product.Name}, category: {pair.Category.Name}");
            }
        }

        private static void PrintSeparator(string label)
        {
            Console.WriteLine("------------------------------------");
            Console.WriteLine(label);
            Console.WriteLine("------------------------------------");
        }

        private static void GettingStarted_MyFirstLinqQuery()
        {
            int n = 20;

            /*
            var query = from nr in NumbersGenerator.Generate()
                        where nr % 2 == 0
                        select nr;
            */

            var query = NumbersGenerator.Generate()
                .Where(nr => nr % 2 == 0);

            int i = 0;
            foreach (int nr in query)
            {
                i++;
                Console.WriteLine(nr);
                if (i >= n)
                {
                    break;
                }
            }
        }

        private static void FilterOperators_Where_Example()
        {
            int index = 0;
            foreach (Person p in PersonsDatabase.AllPersons)
            {
                p.Print(index);
                index++;
            }

            PrintSeparator("Query results:");

            /*
            var query = from person in PersonsDatabase.AllPersons
                        where (person.Age > 14) && person.LastName.StartsWith("D", StringComparison.OrdinalIgnoreCase)
                        select person;
            */

            var query = PersonsDatabase.AllPersons
                .Where((person, idx) => (person.Age > 14) &&
                                        person.LastName.StartsWith("D", StringComparison.OrdinalIgnoreCase) &&
                                        (idx % 2 == 1));

            foreach (Person p in query)
            {
                p.Print();
            }
        }

        private static void FilterOperators_OfType_ExampleWithArrayList()
        {
            ArrayList list = new ArrayList();
            list.Add(1);
            list.Add(2);
            list.Add("test1");
            list.Add("test2");
            list.Add(new Person("John", "Doe", new DateTime(1980, 3, 30), Gender.Male));

            IEnumerable<int> numbers = list.OfType<int>();
            foreach (int nr in numbers)
            {
                Console.Write($"{nr}, ");
            }
        }

        private static void FilterOperators_OfType_ExampleWithInheritance()
        {
            int index = 0;
            foreach (Person p in PersonsDatabase.AllPersons)
            {
                p.Print(index);
                index++;
            }

            PrintSeparator("Query results:");

            var query = PersonsDatabase.AllPersons.OfType<Student>();
            foreach (Person p in query)
            {
                p.Print();
            }
        }

        private static void ProjectionOperators_Select_Example()
        {
            int index = 0;
            foreach (Person p in PersonsDatabase.AllPersons)
            {
                p.Print(index);
                index++;
            }

            PrintSeparator("Query results:");

            /*
            var query = from person in PersonsDatabase.AllPersons
                        select person.FullName;
            */

            var query = PersonsDatabase.AllPersons.Select(person => person.FullName);

            foreach (string fullName in query)
            {
                Console.WriteLine(fullName);
            }
        }

        private static void ProjectionOperators_Select_ExampleWithIndex()
        {
            int index = 0;
            foreach (Person p in PersonsDatabase.AllPersons)
            {
                p.Print(index);
                index++;
            }

            PrintSeparator("Query results:");

            /*
            var query = (from person in PersonsDatabase.AllPersons
                        where person.Age > 14
                        select person.FullName)
                        .Select((fullName, idx) => new { FullName = fullName, Index = idx });
            */

            var query = PersonsDatabase.AllPersons
                .Where(person => person.Age > 14)
                .Select((person, idx) => new { FullName = person.FullName, Index = idx });


            foreach (var personAndIndex in query)
            {
                Console.WriteLine($"{personAndIndex.Index} - {personAndIndex.FullName}");
            }
        }

        private static void ProjectionOperators_SelectMany_Example1()
        {
            int[] numbers = { 1, 2, 3, 4 };
            // Result: 1, 1, 1, 2, 4, 8, 3, 9, 27, ...

            /*
            var query = from nr in numbers
                        from powers in new[] { nr, nr * nr, nr * nr * nr }
                        select powers;
            */

            var query = numbers.SelectMany(nr => new[] { nr, nr * nr, nr * nr * nr });

            foreach (int n in query)
            {
                Console.Write($"{n}, ");
            }
        }

        private static void ProjectionOperators_SelectMany_Example2()
        {
            int[] collection1 = { 1, 2, 3, 4 };
            int[] collection2 = { 4, 5 };
            // Result: (1, 4), (1, 5), (2, 4), (2, 5), (3, 4), (3, 5), (4, 4), (4, 5)
            // Result: (3, 4), (4, 5)

            /*
            var query = from elem1 in collection1
                        from elem2 in collection2
                        where Math.Abs(elem1 - elem2) == 1
                        select new Tuple<int, int>(elem1, elem2);
            */

            var query = collection1
                .SelectMany(
                    elem1 => collection2,
                    (elem1, elem2) => new Tuple<int, int>(elem1, elem2))
                .Where(pair => Math.Abs(pair.Item1 - pair.Item2) == 1);

            foreach (var result in query)
            {
                Console.Write($"({result.Item1}, {result.Item2}), ");
            }
        }

        private static void SortingOperators_OrderBy_Example()
        {
            int index = 0;
            foreach (Person p in PersonsDatabase.AllPersons)
            {
                p.Print(index);
                index++;
            }

            PrintSeparator("Query results:");

            /*
            var query = from person in PersonsDatabase.AllPersons
                        where person.Age > 20 && person.Age < 40
                        orderby person.Age ascending, person.LastName descending
                        select person;
            */

            var query = PersonsDatabase.AllPersons
                .Where(person => person.Age > 20 && person.Age < 40)
                .OrderBy(person => person.Age)
                .ThenByDescending(person => person.LastName);

            foreach (Person p in query)
            {
                p.Print();
            }
        }

        private static void GrouppingOperators_GroupBy_Example()
        {
            int index = 0;
            foreach (Person p in PersonsDatabase.AllPersons)
            {
                p.Print(index);
                index++;
            }

            PrintSeparator("Query results:");

            /*
            var query = from person in PersonsDatabase.AllPersons
                        where person.Age > 30
                        group person by person.DateOfBirth.Year into groups
                        orderby groups.Key ascending
                        select groups;
            */

            var query = PersonsDatabase.AllPersons
                .Where(person => person.Age > 30)
                .GroupBy(person => person.DateOfBirth.Year)
                .OrderBy(group => group.Key);

            foreach (IGrouping<int, Person> group in query)
            {
                Console.WriteLine($"Persons born in {group.Key}:");
                foreach (Person p in group)
                {
                    p.Print();
                }
            }
        }

        private static void PartitioningOperators_Take_Example()
        {
            int index = 0;
            foreach (Person p in PersonsDatabase.AllPersons)
            {
                p.Print(index);
                index++;
            }

            PrintSeparator("Query results (1):");


            var query = PersonsDatabase.AllPersons
                .Where(person => person.Age > 30)
                .Take(10);

            foreach (Person p in query)
            {
                p.Print();
            }

            PrintSeparator("Query results (2):");

            var query2 = PersonsDatabase.AllPersons
                .OrderBy(person => person.Age)
                .TakeWhile(person => person.Age < 30);

            foreach (Person p in query2)
            {
                p.Print();
            }
        }

        private static void PartitioningOperators_TakeAndSkip_PagingExample()
        {
            int index = 0;
            var sortedSet = PersonsDatabase.AllPersons
                .OrderBy(person => person.LastName)
                .ThenBy(person => person.FirstName);

            foreach (Person p in sortedSet)
            {
                p.Print(index);
                index++;
            }

            PrintSeparator("Query results:");

            int pageSize = 10;
            int totalPersonsCount = PersonsDatabase.AllPersons.Count();
            int totalPagesCount = totalPersonsCount / pageSize +
                                  (totalPersonsCount % pageSize == 0 ? 0 : 1);

            for (int pageNo = 1; pageNo <= totalPagesCount; pageNo++)
            {
                Console.WriteLine("---------------------");
                Console.WriteLine($"Page: {pageNo}");

                var query = sortedSet.Skip((pageNo - 1) * pageSize)
                                     .Take(pageSize);

                foreach (Person p in query)
                {
                    p.Print();
                }

                Console.WriteLine("---------------------");
            }
        }

        private static void SetOperators_Union_Example()
        {
            Person p1 = new Person("Person1", "LastName1", new DateTime(1985, 3, 15), Gender.Male);
            Person p2 = new Person("Person2", "LastName2", new DateTime(1994, 3, 20), Gender.Female);
            Person p3 = new Person("Person3", "LastName3", new DateTime(1985, 3, 15), Gender.Male);
            Person p4 = new Person("Person2", "LastName2", new DateTime(1994, 3, 20), Gender.Female);

            List<Person> personsList1 = new List<Person>
            {
                p1,
                p2
            };

            List<Person> personsList2 = new List<Person>
            {
                p3,
                p4
            };

            var query = personsList1.Union(personsList2);

            foreach (Person p in query)
            {
                p.Print();
            }

            int[] numbers1 = { 1, 2, 3 };
            int[] numbers2 = { 2, 3, 4 };
            var unionNumbers = numbers1.Union(numbers2);
            foreach (int nr in unionNumbers)
            {
                Console.Write($"{nr}, ");
            }
        }

        private static void SetOperators_Zip_Example()
        {
            int[] numbers = { 1, 2, 3 };
            string[] labels = { "label", "test", "hello", "another" };
            var query = numbers.Zip(labels, (elem1, elem2) => $"{elem2}{elem1}");

            foreach (var element in query)
            {
                Console.Write($"{element}, ");
            }
        }

        private static void AggregateFunctions_Count_Example()
        {
            /*
            int[] array = new[] { 1, 2, 3, 4 };
            Console.WriteLine(array.Length);

            List<string> genericList = new List<string>();
            genericList.Add("test1");
            genericList.Add("test2");
            genericList.AddRange(new[] { "test3", "test4" });
            Console.WriteLine(genericList.Count);


            int nrOfPersons = PersonsDatabase.AllPersons.Count();
            Console.WriteLine(nrOfPersons);
            */

            int nrOfPersons = PersonsDatabase.AllPersons.Count(
                person => person.LastName.StartsWith("D", StringComparison.OrdinalIgnoreCase));

            Console.WriteLine(nrOfPersons);
        }

        private static void AggregateFunctions_Min_Example()
        {
            int[] array = { 5, 4, 3, 2, 1 };
            int min = array.Min();

            Console.WriteLine(min);


            int minAge = PersonsDatabase.AllPersons.Min(person => person.Age);
            var query = PersonsDatabase.AllPersons.Where(person => person.Age == minAge);

            Console.WriteLine($"Min age={minAge}");
            foreach (var person in query)
            {
                person.Print();
            }
        }

        private static void AggregateFunctions_Average_Example()
        {
            int[] array = { 5, 4, 3, 2, 1 };
            double avg = array.Average();
            Console.WriteLine(avg);


            double avgAge = PersonsDatabase.AllPersons.Average(person => person.Age);
            var query = PersonsDatabase.AllPersons.Where(person => person.Age == avgAge);
            Console.WriteLine($"Avg age={avgAge}");
            foreach (var person in query)
            {
                person.Print();
            }
        }

        private static void ElementFunctions_First_Example()
        {
            int[] array = { 5, 4, 3, 2, 1 };
            int first = array.First(nr => nr % 2 == 0);

            // But those will throw!!!
            // int[] array = { 5, 3, 1 };
            // int first = array.First(nr => nr % 2 == 0);

            //int[] array = { };
            //int first = array.First();

            Console.WriteLine($"First nr={first}");

            Person p = PersonsDatabase.AllPersons.First(person => person.LastName.StartsWith("M", StringComparison.OrdinalIgnoreCase));
            // But this will throw!!!
            // Person p = PersonsDatabase.AllPersons.First(person => person.LastName.StartsWith("Q", StringComparison.OrdinalIgnoreCase));
            p.Print();
        }

        private static void ElementFunctions_FirstOrDefault_Example()
        {
            int[] array = { 5, 3, 1, 0 };
            int first = array.FirstOrDefault(nr => nr % 2 == 0);
            Console.WriteLine($"First nr={first}");

            Person p = PersonsDatabase.AllPersons.FirstOrDefault(person => person.LastName.StartsWith("M", StringComparison.OrdinalIgnoreCase));
            if (p is not null)
            {
                p.Print();
            }
            else
            {
                Console.Write("No person matched!");
            }
        }

        private static void ElementFunctions_SingleOrDefault_Example()
        {
            int[] array = { 5, 4, 3 };
            int singleValue = array.SingleOrDefault(nr => nr % 2 == 0);
            Console.WriteLine($"First nr={singleValue}");

            // if multiple persons => will throw!
            Person p = PersonsDatabase.AllPersons.SingleOrDefault(person => person.Age == 45);
            if (p is not null)
            {
                p.Print();
            }
            else
            {
                Console.WriteLine("No matching person of 45 years");
            }
        }

        private static void ElementFunctions_ElementAtOrDefault_Example()
        {
            int[] array = { 5, 4, 3 };
            int value = array.ElementAtOrDefault(10);
            Console.WriteLine(value);
        }

        private static void QuantifierFunctions_Any_Example()
        {
            int[] array = { 5, 3, 1 };
            bool atLeastOne = array.Any(nr => nr % 2 == 0);
            Console.WriteLine(atLeastOne);

            bool anyPerson = PersonsDatabase.AllPersons.Any(person => person.Age == 45);
            if (anyPerson)
            {
                Person p = PersonsDatabase.AllPersons.First(person => person.Age == 45);
                p.Print();
            }
            else
            {
                Console.WriteLine("No person of 45 years");
            }
        }

        private static void QuantifierFunctions_All_Example()
        {
            bool allPersons = PersonsDatabase.AllPersons.All(person => person.Gender == Gender.Female);
            if (allPersons)
            {
                Console.WriteLine("All persons are female");
            }
            else
            {
                Console.WriteLine("Not all persons are female");
            }
        }

        private static void QuantifierFunctions_Contains_Example()
        {
            int[] array = { 1, 2, 3, 4 };
            bool containsNr = array.Contains(3);
            Console.WriteLine(containsNr);

            Person p = PersonsDatabase.AllPersons.First();
            bool containsPerson = PersonsDatabase.AllPersons.Contains(p);
            Console.WriteLine(containsPerson);

            Person clone = p.Clone();
            bool containsClone = PersonsDatabase.AllPersons.Contains(clone);
            Console.WriteLine(containsClone);
        }

        private static void QuantifierFunctions_ContainsWithComparer_Example()
        {
            int[] array = { 1, 2, 3, 4 };
            bool containsNr = array.Contains(3);
            Console.WriteLine(containsNr);

            Person p = PersonsDatabase.AllPersons.First();
            bool containsPerson = PersonsDatabase.AllPersons.Contains(p);
            Console.WriteLine(containsPerson);

            Person clone = p.Clone();
            bool containsClone = PersonsDatabase.AllPersons.Contains(
                clone,
                new LambdaEqualityComparer<Person>(
                    (x, y) => (x is not null) &&
                              (y is not null) &&
                              string.Equals(x.FirstName, y.FirstName, StringComparison.OrdinalIgnoreCase) &&
                              string.Equals(x.LastName, y.LastName, StringComparison.OrdinalIgnoreCase) &&
                              x.Gender == y.Gender &&
                              x.DateOfBirth == y.DateOfBirth));

            Console.WriteLine(containsClone);
        }

        private static void Joins_Simple_Example()
        {
            /*
            var query = from product in ProductsDatabase.AllProducts
                        join category in ProductsDatabase.AllCategories on product.CategoryId equals category.Id
                        select new
                        {
                            Product = product,
                            Category = category
                        };
            */

            var query = ProductsDatabase.AllProducts
                .Join(
                    ProductsDatabase.AllCategories,
                    product => product.CategoryId,
                    category => category.Id,
                    (product, category) => new
                    {
                        Product = product,
                        Category = category
                    });

            foreach (var pair in query)
            {
                Console.WriteLine($"{pair.Product.Id}) {pair.Product.Name}, category: {pair.Category.Name}");
            }
        }

        private static void Joins_GroupJoin_Example()
        {
            /*
            var query = from category in ProductsDatabase.AllCategories
                        join product in ProductsDatabase.AllProducts on category.Id equals product.CategoryId into categoryGroup
                        select new
                        {
                            Id = category.Id,
                            CategoryName = category.Name,
                            Products = categoryGroup
                        };
            */

            var query = ProductsDatabase.AllCategories.GroupJoin(
                ProductsDatabase.AllProducts,
                category => category.Id,
                product => product.CategoryId,
                (category, categoryGroup) => new
                {
                    Id = category.Id,
                    CategoryName = category.Name,
                    Products = categoryGroup
                });

            foreach (var group in query)
            {
                Console.WriteLine($"{group.Id}) {group.CategoryName}");
                foreach (var product in group.Products)
                {
                    Console.WriteLine($"    - {product.Id}) {product.Name}");
                }
            }
        }
    }
}
