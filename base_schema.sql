CREATE TABLE brands(
	id SERIAL PRIMARY KEY,
	name VARCHAR
);

CREATE TABLE customers(
	id SERIAL PRIMARY KEY,
	surname VARCHAR,
	name VARCHAR,
	city VARCHAR,
	email VARCHAR
);

CREATE TABLE products(
	id SERIAL PRIMARY KEY,
	name VARCHAR,
	price DECIMAL,
	brand_id INTEGER,
	stock INTEGER,
	CONSTRAINT fk_brand FOREIGN KEY(brand_id) REFERENCES brands(id)
);

CREATE TABLE orders(
	id SERIAL PRIMARY KEY,
	date VARCHAR,
	customer_id INTEGER,
	product_id INTEGER,
	CONSTRAINT fk_customer FOREIGN KEY(customer_id) REFERENCES customers(id),
	CONSTRAINT fk_product FOREIGN KEY(product_id) REFERENCES products(id)
);