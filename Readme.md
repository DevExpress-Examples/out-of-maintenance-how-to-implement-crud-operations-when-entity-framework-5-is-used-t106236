<!-- default file list -->
*Files to look at*:

* **[Form1.cs](./CS/EntityCRUD/Form1.cs) (VB: [Form1.vb](./VB/EntityCRUD/Form1.vb))**
<!-- default file list end -->
# How to implement CRUD operations when Entity Framework 5+ is used


<p>This example illustrates how to implement CRUD operations with XtraGrid and EF 5. This approach is also applicable for Entity Framework 6.</p>
<p> To get a possibility to commit changes, it is necessary to store DbContext instance that is used to get the grid's data source. Then when necessary, you can commit changes to the data base using the DbContext.SaveChanges method </p>

<br/>


