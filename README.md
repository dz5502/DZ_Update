# DZ_Update
由于找了很久都没有找到一款比较好用的客户端更新工具，最后决定自己写一个。该项目旨在成为一个windows平台上完美的更新工具。
服务器端使用DUFS作为http文件服务器，DUFS较简单在此就不多余说明了。
暂时未适配GUI，后续打算使用WPF做一个配套的界面。

已支持服务器文件层级管理及自动生成json说明文件的自动化工具。

## 支持功能
1. 权限控制（配合DUFS）
2. 跨版本更新
3. 回退上一版本
4. 本地文件全量更新
5. 针对客户端不同版本类型更新
6. 针对固定用户更新
7. 强制更新
8. 支持文件下载进度显示

## 文件固有约定
### 文件层级说明
版本文件夹列表按 X.X.X.X 命名，依次递增。额外包含一个所有文件清单，包含最新版本号及每个文件的最新版本，用于全量更新及最新版本获取。

<img width="487" height="249" alt="image" src="https://github.com/user-attachments/assets/f9543be7-a2b2-48ca-ad77-d1d1c461ae0f" />

### 版本文件夹文件目录说明
版本文件夹中包含一个更新文件说明的json文件，一个包含所有需要更新文件的zip文件及所有需要更新的文件（只用于全量修复），正常更新下载zip文件节省带宽

<img width="711" height="232" alt="image" src="https://github.com/user-attachments/assets/df223ac2-3b0c-4bb6-8769-889e4256cf4f" />

### 根目录updateAll.json说明

<img width="1052" height="770" alt="image" src="https://github.com/user-attachments/assets/a0a3c323-7fd7-4171-83e2-57086c2f834a" />

### 版本目录update.json说明

<img width="882" height="561" alt="image" src="https://github.com/user-attachments/assets/b7f5dee0-0c9d-4a03-be76-fe438fb56bf1" />


## 运行效果
<img width="1155" height="604" alt="QQ20250917-154533" src="https://github.com/user-attachments/assets/6f512881-0478-4ee7-af8a-7cbdfef72e4f" />

# DZ_Update_ServerFileManager
用于构建服务器端更新文件，保持文件结构与客户端固定约束。自动生成更新说明json文件。

<img width="1284" height="813" alt="image" src="https://github.com/user-attachments/assets/f8e2fd96-793d-4fdb-bb0d-aa166b8ec907" />

