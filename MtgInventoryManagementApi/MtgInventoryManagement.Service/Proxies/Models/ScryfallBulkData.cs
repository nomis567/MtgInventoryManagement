namespace MtgInventoryManagementApi.MtgInventoryManagement.Service.Proxies.Models;

public record ScryfallBulkData(
	IList<Data> Data
);

public record Data(
    string Id,
    string Type,
    DateTime UpdatedAt,
    string Uri,
    string Name,
    string Description,
    long Size,
    string Download_Uri,
    string Content_Type,
    string Content_Encoding
);
