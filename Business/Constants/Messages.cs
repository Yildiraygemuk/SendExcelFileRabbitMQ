using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Constants
{
    public static class Messages
    {
        public static string BookAdded = "Book successfully added.";
        public static string BookUpdated = "Book successfully updated.";
        public static string BookDeleted = "Book successfully deleted.";
        public static string BookNotFound = "Book not found.";

        public static string CategoryAdded = "Category successfully added.";
        public static string CategoryUpdated = "Category successfully updated.";
        public static string CategoryDeleted = "Category successfully deleted.";
        public static string CategoryNotFound = "Category not found.";

        public static string UserNotFound = "User not exist.";
        public static string UserAlreadyExist = "User already exist";
        public static string PasswordError = "Wrong Password";
        public static string SuccessfulLogin = "Login Successful";
        public static string UserRegistered = "User successfully registered";
        public static string AccesesTokenCreated = "Access Token successfully created";
        public static string AuthorizationDenied = "You are not authorized";

        public static string BookNameAlreadyExists = "Book already exist";
    }
}
