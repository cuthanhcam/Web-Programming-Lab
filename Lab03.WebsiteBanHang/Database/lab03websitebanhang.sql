-- Chèn dữ liệu mẫu vào bảng Category
INSERT INTO Categories(Name) VALUES 
(N'Điện thoại'),
(N'Laptop'),
(N'Máy tính bảng'),
(N'Phụ kiện'),
(N'Thiết bị thông minh');

-- Chèn dữ liệu mẫu vào bảng Product
INSERT INTO Products(Name, Price, Description, CategoryId) VALUES 
-- Sản phẩm cho danh mục "Điện thoại" (CategoryId = 1)
(N'iPhone 15', 999, N'Điện thoại Apple iPhone 15 chính hãng', 1),
(N'Samsung Galaxy S23', 899, N'Điện thoại Samsung flagship 2023', 1),
(N'Xiaomi 13 Pro', 749, N'Điện thoại Xiaomi cao cấp', 1),
(N'OPPO Find X6', 679, N'Điện thoại OPPO với camera đỉnh cao', 1),
(N'Vivo X90', 599, N'Điện thoại Vivo với hiệu năng mạnh mẽ', 1),

-- Sản phẩm cho danh mục "Laptop" (CategoryId = 2)
(N'MacBook Pro M2', 1999, N'Laptop Apple MacBook Pro chip M2', 2),
(N'Dell XPS 15', 1799, N'Laptop Dell XPS màn hình OLED 4K', 2),
(N'Asus ROG Zephyrus G14', 1599, N'Laptop gaming Asus ROG', 2),
(N'HP Spectre x360', 1499, N'Laptop HP Spectre 2-in-1', 2),
(N'Lenovo ThinkPad X1 Carbon', 1999, N'Laptop doanh nhân ThinkPad X1', 2),

-- Sản phẩm cho danh mục "Máy tính bảng" (CategoryId = 3)
(N'iPad Pro M2', 1099, N'Máy tính bảng iPad Pro chip M2', 3),
(N'Samsung Galaxy Tab S8', 899, N'Tablet Samsung màn hình AMOLED', 3),
(N'Xiaomi Pad 6', 499, N'Máy tính bảng giá tốt từ Xiaomi', 3),
(N'Huawei MatePad Pro', 699, N'Tablet Huawei với bút cảm ứng', 3),
(N'Lenovo Tab P12 Pro', 799, N'Máy tính bảng Lenovo cao cấp', 3),

-- Sản phẩm cho danh mục "Phụ kiện" (CategoryId = 4)
(N'AirPods Pro 2', 249, N'Tai nghe không dây Apple AirPods Pro 2', 4),
(N'Samsung Galaxy Buds2 Pro', 229, N'Tai nghe không dây Samsung', 4),
(N'Logitech MX Master 3S', 99, N'Chuột không dây Logitech cao cấp', 4),
(N'Razer BlackWidow V4', 179, N'Bàn phím cơ Razer gaming', 4),
(N'Anker PowerCore 20000mAh', 79, N'Sạc dự phòng Anker dung lượng lớn', 4),

-- Sản phẩm cho danh mục "Thiết bị thông minh" (CategoryId = 5)
(N'Apple Watch Series 9', 399, N'Đồng hồ thông minh Apple Watch', 5),
(N'Samsung Galaxy Watch 5', 349, N'Đồng hồ thông minh Samsung', 5),
(N'Google Nest Hub', 129, N'Màn hình thông minh Google Nest', 5),
(N'Amazon Echo Dot 5', 99, N'Loa thông minh Amazon Alexa', 5),
(N'Xiaomi Mi Smart Air Purifier 4', 199, N'Máy lọc không khí Xiaomi', 5);

SELECT * FROM Categories;
