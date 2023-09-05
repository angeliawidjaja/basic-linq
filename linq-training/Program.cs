using linq_training.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_training
{
    class Program
    {
        //2) Buat local DB
        //Tekan alt + enter kalau cacing merah
        private static List<Student> studentList = new List<Student>
        {
            new Student() { StudentId="001", StudentName="Kornel", StudentAge=19, StudentGender="M"},
            new Student() { StudentId="002", StudentName="Billy", StudentAge=18, StudentGender="F"},
            new Student() { StudentId="003", StudentName="Jokris", StudentAge=17, StudentGender="M"},
            new Student() { StudentId="004", StudentName="Adrian", StudentAge=20, StudentGender="M"},
            new Student() { StudentId="005", StudentName="Angel", StudentAge=18, StudentGender="F"},
            new Student() { StudentId="006", StudentName="Philip", StudentAge=20, StudentGender="M"},
            new Student() { StudentId="007", StudentName="Japar", StudentAge=17, StudentGender="F"}
        };

        private static List<Course> courseList = new List<Course>
        {
            new Course() { StudentId="001", Courses=new List<string>{ "COMP001", "COMP123"  }, CourseId="100" },
            new Course() { StudentId="003", Courses=new List<string>{ "COMP021", "COMP123"  }, CourseId="200" },
            new Course() { StudentId="005", Courses=new List<string>{ "COMP002", "COMP012"  }, CourseId="300" },
            new Course() { StudentId="007", Courses=new List<string>{ "COMP003", "COMP001"  }, CourseId="400" },
            new Course() { StudentId="007", Courses=new List<string>{ "COMP021", "COMP123"  }, CourseId="200" }
        };

        static void Main(string[] args)
        {
            // cek student list udah ada apa belum
            // foreach (Student i in studentList)
            // {
            //    Console.WriteLine(i.StudentName);
            // }

            // linqSelect();
            // linqWhere();
            // linqFirstOrDefault();
            // linqOrderBy();
            // linqDistinct();
            // linqContains();
            // linqAllAny();
            //linqCountMax();
            //linqGroupBy();
            //linqSelectMany();
            //linqJoin();
            linqGroupJoin();
        }

        // ACTUAL LINQ QUERY
        // ToList, semua method di linq return IEnumerable
        // 1) Select
        // SQL: select StudentName, StudentAge from studentList
        static void linqSelect()
        {
            // data ini anonymous object ('a) yang tidak dikenal, Student adalah object dengan nama
            var data = studentList.Select(s => new { s.StudentName, s.StudentAge } ).ToList();
            foreach(var i in data)
            {
                Console.WriteLine($"> studentName: {i.StudentName}, studentAge: {i.StudentAge}");
            }
        }

        // 2) Where
        // SQL: select * from studentList where StudentAge >= 18
        static void linqWhere()
        {
            var data = studentList.Where(
                p => p.StudentAge >= 18 && 
                p.StudentAge < 20 && 
                p.StudentGender == "F"
                ).ToList();
            foreach (var i in data)
            {
                Console.WriteLine(i.StudentName);
            }
        }

        // 3) FirstOrDefault, return 1 object saja
        static void linqFirstOrDefault()
        {
            Student data = studentList.FirstOrDefault();
            Console.WriteLine($"> {data.StudentName} {data.StudentId}");
        }

        // 4) OrderBy, urutkan data
        static void linqOrderBy()
        {
            // Sort by age
            var data = studentList.OrderBy(p => p.StudentAge).ToList();
            foreach (var i in data)
            {
                Console.WriteLine(i.StudentAge);
            }
        }

        // 5) Distinct
        static void linqDistinct()
        {
            // Ga bisa distinct object, misal object Student
            // Bisa pakai overload dengan masukkan parameter pembanding di dalam Distinct()   
            var data = studentList.Select(s => s.StudentGender).Distinct().ToList();
            foreach (string i in data)
            {
                Console.WriteLine(i);
            }
        }

        // 6) Contains, cek apakah sebuah list A mengandung sebuah item B
        static void linqContains()
        {
            List<string> blacklistedStudent = new List<string> { "Japar", "Billy" };
            var data = studentList.
                Where(p => blacklistedStudent.Contains(p.StudentName)).
                Select(s => s.StudentId).ToList();
            foreach (string i in data)
            {
                Console.WriteLine(i);
            }
        }

        // 7) All & Any
        static void linqAllAny()
        {
            // Cek apakah semua data Male
            bool testAll = studentList.All(p => p.StudentGender == "M");
            Console.WriteLine(testAll);

            // Cek apakah ada salah satu data yang Male
            bool testAny = studentList.Any(p => p.StudentGender == "M");
            Console.WriteLine(testAny);
        }

        // 8) Count & Max
        static void linqCountMax()
        {
            // Count
            // Hitung berapa data di studentList
            // Ini Count pakai Method dari linq
            int linqCount = studentList.Count();
            // Ini Count pakai Property/Variable Count dari class List
            int propCount = studentList.Count;
            Console.WriteLine(linqCount);
            Console.WriteLine(propCount);

            // Max
            // Ambil value maksimum
            var data = studentList.Select(s => s.StudentAge).ToList();
            Console.WriteLine(data.Max());
        }

        // 9) GroupBy, kebagi jadi group tapi ga bisa langsung ambil value di dalam groupnya, harus pakai fungsi agregasi
        static void linqGroupBy()
        {
            // Sort by age
            var data = studentList.GroupBy(p => p.StudentGender).ToList();
            foreach (var i in data)
            {
                // ada 2 property di iGroup = key & object
                Console.WriteLine(i.Key);
                foreach(Student j in i)
                {
                    Console.WriteLine(j.StudentName);
                }
            }
        }

        //10) Select Many, list A yg tiap itemnya ada property list B. Kita mau ambil list B pakai Select Many
        static void linqSelectMany()
        {
            var data = courseList.SelectMany(s => s.Courses).Distinct().ToList();
            foreach(string i in data)
            {
                Console.WriteLine(i);
            }
        }

        // 11) Join, inner join biasa, ini mappingnya 1 per 1
        static void linqJoin()
        {
            var data = studentList.Join(courseList,
                keyStudentList => keyStudentList.StudentId,
                keyCourseList => keyCourseList.StudentId,
                (s1, s2) => new
                {
                    s1.StudentId,
                    s1.StudentName,
                    s1.StudentGender,
                    s2.Courses
                }
            ).ToList();

            foreach(var i in data)
            {
                Console.WriteLine($"> {i.StudentId}, {i.StudentName}, {i.StudentGender} Courses: ");
                foreach(string j in i.Courses)
                {
                    Console.WriteLine(j);
                }
            }
        }

        // 12) Group Join, left (or right) join, mappingnya 1 kiri untuk array kanan
        static void linqGroupJoin()
        {
            var data = studentList.GroupJoin(courseList,
                leftKey => leftKey.StudentId,
                rightKey => rightKey.StudentId,
                ( s1,s2 ) => new
                {
                    s1.StudentId,
                    s1.StudentName,
                    s2
                }
            );
            foreach(var i in data)
            {
                Console.WriteLine(i.StudentId);
                if(i.s2.Count() != 0)
                {
                    Console.WriteLine($"User has {i.s2.Count()}. Those courses set are: ");
                    foreach (var j in i.s2)
                    {
                        Console.WriteLine($"-");
                        foreach(string coursecode in j.Courses)
                        {
                            Console.WriteLine($" {coursecode}");
                        }
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("No course for specified student!");
                }
            }
        }
    }
}
