## Overview

The SHIELD web application is a robust and secure platform developed using ASP.NET Core API, leveraging Microsoft Identity for comprehensive user and role management functionalities. The app is designed to manage user registrations, logins, roles, and permissions seamlessly, ensuring a secure and user-friendly experience.

## Functional Requirements

### User Management

- **User Registration:** Allow users to register by providing necessary details like email, password, and other optional information.
- **User Login:** Authenticate users using credentials (email and password).
- **Change Password:** Enable users to update their password securely.
- **Change Email:** Allow users to change their registered email address.
- **Delete Account:** Provide users with the ability to delete their accounts if necessary.
- **Profile Management:** Enable users to view and update their profile information.
- **Email Confirmation:** Verify user email addresses to enhance security.
- **Confirm Email Change:** Confirm the updated email address to ensure it is valid.

### Role Management

- **Create Role:** Admin users can create new roles (e.g., Admin, User, Moderator) and assign them specific permissions.
- **Delete Role:** Admin users can remove roles that are no longer needed.
- **Assign Role to User:** Admin users can assign specific roles to users, defining their permissions and access levels within the application.
- **Remove Role from User:** Admin users can revoke roles from users if necessary.

## Non-Functional Requirements

- **Security:** Implement robust authentication and authorization mechanisms using Microsoft Identity to protect user data and ensure secure access control.
- **Performance:** Optimize the application for quick response times and efficient handling of user requests.
- **Scalability:** Ensure the app can handle an increasing number of users and roles without significant performance degradation.
- **Maintainability:** Develop the application with clean code and best practices to facilitate future updates and maintenance.

## Use Cases

1. **User Registration and Login:** New users can sign up by providing their email and password. After email verification, they can log in to access the application.
2. **Role Assignment:** Admins can create roles and assign them to users based on their responsibilities within the application.
3. **Profile Management:** Users can update their profile details, such as changing their password or email address.
4. **Account Deletion:** Users have the option to delete their accounts if they no longer wish to use the application.

## Technologies Used

- **ASP.NET Core API:** For backend development and API creation.
- **Microsoft Identity:** For handling authentication and authorization.
- **Entity Framework Core:** For database operations and ORM functionality.
- **SQL Server:** As the database to store user and role information.
