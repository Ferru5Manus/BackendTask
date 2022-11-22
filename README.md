# BackendTask
Подготовка к работе(Linux server):
-----------
1. В первую очередь для развёртывания решения вам понадобится docker, руководство по установке находится здесь:
```
https://docs.docker.com/desktop/install/linux-install/
```
2. Далее нужно скачать проект в выбранную вами директорию, с помощью команды:
```
git clone https://github.com/Romario1222/BackendTask
```
3. После переходим в директорию с названием BackendTask, после чего прописываем команду:
```
docker compose up --build
```
4. Проект развёрнут.

Работа с запросами:
-----------
0. Данные отправляемые на сервер, проходят валидацию, в случае ошибки валидации ответ сервера будет содержать текст ошибки.Требования к отправляемым данным:
	 1. id айди пользователя - int,
	 2. username - string, строка не должна содержать следующие символы \"';<>?/{}[]@#$%^&*()-+=_ и пробел, длина строки должна составлять от 8 до 50 символов.
	 3. password - string, строка не должна содержать следующие символы \"';<>?/{}[]@#$%^&*()-+=_ и пробел, длина строки должна составлять от 8 до 50 символов.

1. Для получения пользователя через id в POST запросе /getUser в виде json надо отправить id (id:(айди пользователя(число))), в качестве ответа будет получен записаный в json ValidatedUserDto, в случае возникновения ошибки, ответ сервера будет содержать текст ошибки.
2. Для получения пользователя через имя пользователя в POST запросе /getUser в виде json надо отправить username (username:(имя пользователя(строка))), в качестве ответа будет получен записаный в json ValidatedUserDto, в случае возникновения ошибки, ответ сервера будет содержать текст ошибки.
3. Для создания пользователя нужно в PUT запросе /createUser отправить username и password в виде json, в случае ошибки при создании пользователя ответ сервера будет содержать текст ошибки, при правильном выполнении ответ сервера содержит Added user!
4. Для удаления пользователя нужно в DELETE запросе /deleteUser отправить id пользователя в виде json, в случае ошибки при удалении пользователя ответ сервера будет содержать текст ошибки, при правильном выполнении ответ сервера содержит Deleted user!
5. Для изменения пользователя нужно в запросе /updateUser отправить id, username, password пользователя в виде json, в случае ошибки при обновлении пользователя ответ сервера содержит текст ошибки, при правильном выполнении ответ сервера содержит Updated user!.

Работа с базой данных:
-----------
1. Работу с базой данных осуществляет класс UserDatabaseRepository.
1.1 Для проверки наличия таблицы используется метод IsTableExist().
1.2 Для создания таблицы в случае её отсутствия используется метод ConfigureDb().
2. Для записи новой строки в базу данных используется метод CreateUser, принимающий на вход проверенный экземпляр класса ValidatedUserDto, после записывает в базу данных нового пользователя. В случае ошибки записи в базу данных, в консоль будет написан текст ошибки, его же вернёт метод. В случае правильного выполнения метод вернёт Added user!
3. Для обновления данных в строке в базе данных используется метод UpdateUser, принимающий на вход проверенный экземпляр класса ValidatedUserDto, после изменяет данные пользователя с соответствущим Id. Для обновления доступны поля username,password. В случае ошибки обновления данных в базе данных, в консоль будет написан текст ошибки, его же вернёт метож. В случае ошибки записи в базу данных, в консоль будет написан текст ошибки, его же вернёт метод. В случае правильного выполнения метод вернёт Updated user!
4. Для удаления из строки из базы данных используется метод DeleteUser, принимающий на вход проверенный экземпляр класса ValidatedUserDto, после удаляет пользователя с соответствущим Id. В случае ошибки удаления записи в базе данных, в консоль будет написан текст ошибки, его же вернёт метод. В случае правильного выполнения метод вернёт Deleted user!
5. Для получения данных о пользователе используются методы GetUserByName, GetUserById, первый принимает на вход имя пользователя, а второй id пользователя.После этого из базы данных получается пользователь с соответствующим именем пользователя/id. Метод возвращает объект ValidatedUserDto.В случае ошибки получения данных в консоль будет выведена ошибка, а метод вернёт null.
