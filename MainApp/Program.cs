using System;
using Infrastructure.Models;
using Infrastructure.Services;

class Program
{
    static void Main()
    {
        var doctorService = new DoctorService();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Меню управления докторами");
            Console.WriteLine("1. Показать всех докторов");
            Console.WriteLine("2. Добавить доктора");
            Console.WriteLine("3. Обновить доктора");
            Console.WriteLine("4. Удалить доктора");
            Console.WriteLine("0. Выход");
            Console.Write("Введите команду: ");

            int command = Convert.ToInt32(Console.ReadLine());

            if (command == 1)
            {
                var doctors = doctorService.GetAllDoctors();
                foreach (var doctor in doctors)
                {
                    Console.WriteLine("------------------------------------------------------");
                    Console.WriteLine($"ID: {doctor.DoctorID}");
                    Console.WriteLine($"Имя: {doctor.FirstName} {doctor.LastName}");
                    Console.WriteLine($"Дата рождения: {doctor.DateOfBirth.ToString("yyyy-MM-dd")}");
                    Console.WriteLine($"Должность: {doctor.Position}");
                    Console.WriteLine($"Отдел: {doctor.Department}");
                    Console.WriteLine($"Дата приема на работу: {doctor.HireDate.ToString("yyyy-MM-dd")}");
                    Console.WriteLine($"Зарплата: {doctor.Salary:C}");
                    Console.WriteLine($"Активен: {(doctor.IsActive ? "Да" : "Нет")}");
                    Console.WriteLine("------------------------------------------------------");
                }
            }
            else if (command == 2)
            {
                var doctor = new Doctor();

                Console.Write("Введите имя: ");
                doctor.FirstName = Console.ReadLine();

                Console.Write("Введите фамилию: ");
                doctor.LastName = Console.ReadLine();

                Console.Write("Введите дату рождения (гггг-мм-дд): ");
                doctor.DateOfBirth = DateTime.Parse(Console.ReadLine());

                Console.Write("Введите должность: ");
                doctor.Position = Console.ReadLine();

                Console.Write("Введите отдел: ");
                doctor.Department = Console.ReadLine();

                Console.Write("Введите дату приема на работу (гггг-мм-дд): ");
                doctor.HireDate = DateTime.Parse(Console.ReadLine());

                Console.Write("Введите зарплату: ");
                doctor.Salary = decimal.Parse(Console.ReadLine());

                Console.Write("Активен ли доктор? (да/нет): ");
                doctor.IsActive = Console.ReadLine()?.ToLower() == "да";

                int newDoctorId = doctorService.AddDoctor(doctor);
                Console.WriteLine($"Доктор успешно добавлен с ID: {newDoctorId}");
            }
            else if (command == 3)
            {
                Console.Write("Введите ID доктора для обновления: ");
                int doctorId = Convert.ToInt32(Console.ReadLine());

                var doctor = doctorService.GetDoctorById(doctorId);
                if (doctor != null)
                {
                    Console.Write($"Имя ({doctor.FirstName}): ");
                    doctor.FirstName = Console.ReadLine() ?? doctor.FirstName;

                    Console.Write($"Фамилия ({doctor.LastName}): ");
                    doctor.LastName = Console.ReadLine() ?? doctor.LastName;

                    Console.Write($"Дата рождения ({doctor.DateOfBirth.ToString("yyyy-MM-dd")}): ");
                    string dob = Console.ReadLine();
                    doctor.DateOfBirth = string.IsNullOrWhiteSpace(dob) ? doctor.DateOfBirth : DateTime.Parse(dob);

                    Console.Write($"Должность ({doctor.Position}): ");
                    doctor.Position = Console.ReadLine() ?? doctor.Position;

                    Console.Write($"Отдел ({doctor.Department}): ");
                    doctor.Department = Console.ReadLine() ?? doctor.Department;

                    Console.Write($"Зарплата ({doctor.Salary:C}): ");
                    string salary = Console.ReadLine();
                    doctor.Salary = string.IsNullOrWhiteSpace(salary) ? doctor.Salary : decimal.Parse(salary);

                    Console.Write($"Активен ({(doctor.IsActive ? "Да" : "Нет")}): ");
                    string active = Console.ReadLine();
                    doctor.IsActive = string.IsNullOrWhiteSpace(active) ? doctor.IsActive : active.ToLower() == "да";

                    if (doctorService.UpdateDoctor(doctor))
                    {
                        Console.WriteLine("Данные доктора успешно обновлены.");
                    }
                    else
                    {
                        Console.WriteLine("Ошибка обновления данных.");
                    }
                }
                else
                {
                    Console.WriteLine("Доктор с таким ID не найден.");
                }
            }
            else if (command == 4)
            {
                Console.Write("Введите ID доктора для удаления: ");
                int doctorId = Convert.ToInt32(Console.ReadLine());

                if (doctorService.DeleteDoctor(doctorId))
                {
                    Console.WriteLine("Доктор успешно удален.");
                }
                else
                {
                    Console.WriteLine("Ошибка удаления. Возможно, доктор с таким ID не существует.");
                }
            }
            else if (command == 0)
            {
                break;
            }
        }
    }
}
