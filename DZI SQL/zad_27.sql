CREATE DATABASE hospital;

USE hospital;

CREATE TABLE departments
(
	id INT PRIMARY KEY IDENTITY,
	department_name NVARCHAR(255) NOT NULL,
	count_beds INT NOT NULL
);

CREATE TABLE patients
(
	id INT PRIMARY KEY IDENTITY,
	name NVARCHAR(255) NOT NULL,
	age INT NOT NULL,
	diagnosis NVARCHAR(255) NOT NULL,
	department_id INT NOT NULL,
		CONSTRAINT fk_patients_departments
			FOREIGN KEY (department_id)
			REFERENCES departments(id)
);

INSERT INTO departments(department_name,count_beds)
VALUES
('��������',25),
('���������',18),
('���������',35),
('������������',20);

INSERT INTO patients(name,age,diagnosis,department_id)
VALUES
('�������� ������',36,'��������',2),
('��� ��������',4,'�����',3),
('������ �����',67,'���������',4),
('���� ���������',27,'��������',2),
('������ ������',53,'������',1),
('�������� �����',12,'���������',3);

SELECT name,diagnosis FROM patients AS p
JOIN departments AS d ON d.id=p.department_id
WHERE p.age>=20 AND p.age<=60
ORDER BY p.age DESC;


SELECT name,age,d.department_name FROM patients AS p
JOIN departments AS d ON d.id=p.department_id;

SELECT d.id, COUNT(*) AS 'people count' FROM patients AS p
JOIN departments AS d ON d.id=p.department_id
GROUP BY d.id
ORDER BY COUNT(*);