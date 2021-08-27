<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128629679/18.2.2%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T106236)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
# How to implement CRUD operations when Entity Framework 5+ is used


<p>This example illustrates how to implement CRUD operations with XtraGrid and EF 5. This approach is also applicable for Entity Framework 6.</p>
<p> To get a possibility to commit changes, it is necessary to store DbContext instance that is used to get the grid's data source. Then when necessary, you can commit changes to the data base using the DbContext.SaveChanges method </p>

<br/>


