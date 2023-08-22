namespace ZooWeb;

public class SortColumn
{
	public string? ColumnName { get; set; }

	public string? SortExpression { get; set; }
	public string? SortIcon { get; set; }

}

public class SortModel
{
	private const string UP_ICON = "fa fa-arrow-up";
	private const string DOWN_ICON = "fa fa-arrow-down";
	public string? PropertyName { get; set; }
	public SortOrder SortOrder { get; set; }


	private List<SortColumn> _columns = new List<SortColumn>();

	public SortModel(params string[] column)
	{
		foreach (var name in column)
		{
			AddColumn(name);

		}
	}


	public void AddColumn(string colname, bool isDefault = false)
	{
		colname = colname.ToLower();
		if (_columns.Where(c => c.ColumnName == colname).FirstOrDefault() == null)
		{

			_columns.Add(new SortColumn
			{
				ColumnName = colname,
				SortExpression = colname,
				SortIcon = ""

			});
		}
		if (_columns.Count == 1 || isDefault)
		{
			PropertyName = colname;
			SortOrder = SortOrder.Ascending;
		}

	}
	public SortColumn? GetColumn(string colname)
	{
		colname = colname.ToLower();
		return _columns.Where(c => c.ColumnName == colname).FirstOrDefault();
	}

	private void applySort(string sortExprission)
	{
		if (string.IsNullOrEmpty(sortExprission) || !_columns.Any(a => a.ColumnName == sortExprission || a.ColumnName + "_desc" == sortExprission))
			sortExprission = PropertyName + "_desc";
		sortExprission = sortExprission.ToLower();
		foreach (var col in _columns)
		{
			col.SortIcon = "";
			col.SortExpression = col.ColumnName;
			if (col.ColumnName == sortExprission)
			{
				PropertyName = col.ColumnName;
				SortOrder = SortOrder.Ascending;
				col.SortIcon = DOWN_ICON;
				col.SortExpression = col.ColumnName + "_desc";

			}
			if (col.ColumnName + "_desc" == sortExprission)
			{
				PropertyName = col.ColumnName;
				SortOrder = SortOrder.Descending;
				col.SortIcon = UP_ICON;
				col.SortExpression = col.ColumnName;

			}

		}
	}

	private Expression<Func<Animal, object>> filter() => PropertyName switch
		{
			"name" => (Expression<Func<Animal, object>>)(a => a.Name!),
			"age" => (Expression<Func<Animal, object>>)(a => a.Age),
			"commentscount" => (Expression<Func<Animal, object>>)(a => a.Comments!.Count()),
			_ => throw new ArgumentException("Invalid sorting property")
		};

	public Expression<Func<Animal, object>> GetFilter(string sortExprission)
	{
		applySort(sortExprission);
		return filter();
	}

}
