USE soft_gepec;

IF NOT EXISTS (SELECT name FROM sys.tables WHERE name = 'users')
	BEGIN
		CREATE TABLE users (
			id BIGINT PRIMARY KEY,
			email NVARCHAR(255) NOT NULL,
			password NVARCHAR(255) NOT NULL,
			creation_date DATETIME NOT NULL DEFAULT GETUTCDATE(),
			deletion_date DATETIME

		);
	END

IF NOT EXISTS (SELECT name FROM sys.tables WHERE name = 'departments')
	BEGIN
		CREATE TABLE departments (
			id BIGINT PRIMARY KEY,
			title NVARCHAR(255) NOT NULL,
			creation_date DATETIME NOT NULL DEFAULT GETUTCDATE(),
			created_by BIGINT NOT NULL,
			deletion_date DATETIME,
			deleted_by BIGINT,
			FOREIGN KEY (created_by) REFERENCES users(id),
			FOREIGN KEY (deleted_by) REFERENCES users(id)
		);
	END

IF NOT EXISTS (SELECT name FROM sys.tables WHERE name = 'services')
	BEGIN
		CREATE TABLE services (
			id BIGINT PRIMARY KEY,
			title NVARCHAR(255) NOT NULL,
			department_id BIGINT NOT NULL,
			creation_date DATETIME NOT NULL DEFAULT GETUTCDATE(),
			created_by BIGINT NOT NULL,
			deletion_date DATETIME,
			deleted_by BIGINT,
			FOREIGN KEY (department_id) REFERENCES departments(id),
			FOREIGN KEY (created_by) REFERENCES users(id),
			FOREIGN KEY (deleted_by) REFERENCES users(id)
		);
	END

IF NOT EXISTS (SELECT name FROM sys.tables WHERE name = 'employees')
	BEGIN
		CREATE TABLE employees (
			id BIGINT PRIMARY KEY,
			lastName NVARCHAR(255) NOT NULL,
			firstName NVARCHAR(255) NOT NULL,
			entry_date DATETIME NOT NULL,
			confirmation_date DATETIME NOT NULL,
			service_id BIGINT NOT NULL,
			creation_date DATETIME NOT NULL DEFAULT GETUTCDATE(),
			created_by BIGINT NOT NULL,
			deletion_date DATETIME,
			deleted_by BIGINT,
			FOREIGN KEY (service_id) REFERENCES services(id),
			FOREIGN KEY (created_by) REFERENCES users(id),
			FOREIGN KEY (deleted_by) REFERENCES users(id)
		);
	END

IF NOT EXISTS (SELECT name FROM sys.tables WHERE name = 'skills_groups')
	BEGIN
		CREATE TABLE skills_groups (
			id BIGINT PRIMARY KEY,
			title NVARCHAR(255) NOT NULL,
			creation_date DATETIME NOT NULL DEFAULT GETUTCDATE(),
			created_by BIGINT NOT NULL,
			deletion_date DATETIME,
			deleted_by BIGINT,
			FOREIGN KEY (created_by) REFERENCES users(id),
			FOREIGN KEY (deleted_by) REFERENCES users(id)
		);
	END

IF NOT EXISTS (SELECT name FROM sys.tables WHERE name = 'skills')
	BEGIN
		CREATE TABLE skills (
			id BIGINT PRIMARY KEY,
			title NVARCHAR(255) NOT NULL,
			skills_group_id BIGINT NOT NULL,
			creation_date DATETIME NOT NULL DEFAULT GETUTCDATE(),
			created_by BIGINT NOT NULL,
			deletion_date DATETIME,
			deleted_by BIGINT,
			FOREIGN KEY (skills_group_id) REFERENCES skills_groups(id),
			FOREIGN KEY (created_by) REFERENCES users(id),
			FOREIGN KEY (deleted_by) REFERENCES users(id)
		);
	END

IF NOT EXISTS (SELECT name FROM sys.tables WHERE name = 'skills_coefficients')
	BEGIN
		CREATE TABLE skills_coefficients (
			id BIGINT IDENTITY(1, 1) PRIMARY KEY,
			skill_id BIGINT NOT NULL,
			service_id BIGINT NOT NULL,
			coefficient INT NOT NULL,
			creation_date DATETIME NOT NULL DEFAULT GETUTCDATE(),
			created_by BIGINT NOT NULL,
			deletion_date DATETIME,
			deleted_by BIGINT,
			FOREIGN KEY (skill_id) REFERENCES skills(id),
			FOREIGN KEY (service_id) REFERENCES services(id),
			FOREIGN KEY (created_by) REFERENCES users(id),
			FOREIGN KEY (deleted_by) REFERENCES users(id)
		);
	END

IF NOT EXISTS (SELECT name FROM sys.tables WHERE name = 'employees_skills')
	BEGIN
		CREATE TABLE employees_skills (
			id BIGINT IDENTITY(1, 1) PRIMARY KEY,
			employee_id BIGINT NOT NULL,
			skill_id BIGINT NOT NULL,
			mark INT NOT NULL, 
			test_date DATETIME NOT NULL,
			creation_date DATETIME NOT NULL DEFAULT GETUTCDATE(),
			created_by BIGINT NOT NULL,
			deletion_date DATETIME,
			deleted_by BIGINT,
			FOREIGN KEY (employee_id) REFERENCES employees(id),
			FOREIGN KEY (skill_id) REFERENCES skills(id),
			FOREIGN KEY (created_by) REFERENCES users(id),
			FOREIGN KEY (deleted_by) REFERENCES users(id)
		);
	END
