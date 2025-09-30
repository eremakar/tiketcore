﻿using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Tarifications.Models.Queries.Users
{
    public partial class UserFilter : FilterBase<User>
    {
        public FilterOperand<int>? Id { get; set; }
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public FilterOperand<string>? UserName { get; set; }
        /// <summary>
        /// Хеш пароля
        /// </summary>
        public FilterOperand<string>? PasswordHash { get; set; }
        /// <summary>
        /// Активен
        /// </summary>
        public FilterOperand<bool>? IsActive { get; set; }
        /// <summary>
        /// Количество попыток входа
        /// </summary>
        public FilterOperand<int>? ProtectFromBruteforceAttempts { get; set; }
        /// <summary>
        /// ФИО
        /// </summary>
        public FilterOperand<string>? FullName { get; set; }
        /// <summary>
        /// Название должности
        /// </summary>
        public FilterOperand<string>? PositionName { get; set; }
        /// <summary>
        /// Статус: 1 - ожидает кода регистрации, 2 - зарегистрирован, 3 - ввести пин код
        /// </summary>
        public FilterOperand<int>? State { get; set; }
        /// <summary>
        /// Код регистрации
        /// </summary>
        public FilterOperand<string>? RegistrationToken { get; set; }
        /// <summary>
        /// Время истечения блокировки
        /// </summary>
        public FilterOperand<DateTime>? BlockExpiration { get; set; }
        /// <summary>
        /// Пуш токен
        /// </summary>
        public FilterOperand<string>? PushToken { get; set; }
        /// <summary>
        /// Signalr токен
        /// </summary>
        public FilterOperand<string>? SignalrToken { get; set; }
        /// <summary>
        /// Пин код
        /// </summary>
        public FilterOperand<string>? PinCode { get; set; }
        /// <summary>
        /// Время истечения пин кода
        /// </summary>
        public FilterOperand<DateTime>? PinCodeExpiration { get; set; }
        /// <summary>
        /// S3 файл аватара
        /// </summary>
        public FilterOperand<object>? Avatar { get; set; }
        /// <summary>
        /// Защита от взлома
        /// </summary>
        public FilterOperand<int>? FailedLoginCount { get; set; }
        public FilterOperand<int?>? RoleId { get; set; }
    }
}
