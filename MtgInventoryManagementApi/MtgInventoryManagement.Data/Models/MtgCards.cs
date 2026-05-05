using System.ComponentModel.DataAnnotations;

namespace MtgInventoryManagementApi.MtgInventoryManagement.Data.Models;

public class Card
{
	[Key]
	public Guid Id { get; set; }

	[Required]
	public string Name { get; set; } = string.Empty;

	public string? Artist { get; set; }
	public string? Border { get; set; }
	public int Cmc { get; set; }
	
	public List<string> ColorIdentity { get; set; } = new();
	public List<string> Colors { get; set; } = new();
	public List<string> ColorIndicator { get; set; } = new();
	
	public string? Cost { get; set; }
	public string? Flavor { get; set; }
	public string? FrameVersion { get; set; }
	public string Layout { get; set; } = "NORMAL";
	
	public bool HasAlternativeDeckLimit { get; set; }
	public bool Alternative { get; set; }
	public bool Funny { get; set; }
	public bool Rebalanced { get; set; }
	public bool StorySpotlight { get; set; }
	
	public string? Power { get; set; } // String to support "*"
	public string? Toughness { get; set; }
	public int Loyalty { get; set; }
	
	public string Rarity { get; set; } = "COMMON";
	public string? Number { get; set; }
	public string? Text { get; set; }
	public string? Url { get; set; }

	public int MkmId { get; set; }
	public int TcgPlayerId { get; set; }
	public Guid ScryfallId { get; set; }
	public Guid ScryfallIllustrationId { get; set; }

	public virtual ICollection<CardEdition> Editions { get; set; } = new List<CardEdition>();
	public virtual ICollection<ForeignName> ForeignNames { get; set; } = new List<ForeignName>();
	public virtual ICollection<Legality> Legalities { get; set; } = new List<Legality>();
	
	public List<string> Keywords { get; set; } = new();
	public List<string> Subtypes { get; set; } = new();
	public List<string> Supertypes { get; set; } = new();
	public List<string> Types { get; set; } = new();
	public List<string> Finishes { get; set; } = new();
}

public class Edition
{
	[Key]
	public string Id { get; set; } = string.Empty; 

	public string Set { get; set; } = string.Empty;
	public DateTime ReleaseDate { get; set; }
	public string? Type { get; set; }
	public int CardCount { get; set; }
	public string? KeyRuneCode { get; set; }
	public List<string> Booster { get; set; } = new();
	
	public virtual ICollection<CardEdition> CardEditions { get; set; } = new List<CardEdition>();
}

public class CardEdition
{
	public Guid CardId { get; set; }
	public virtual Card Card { get; set; } = null!;

	public string EditionId { get; set; } = string.Empty;
	public virtual Edition Edition { get; set; } = null!;
}

public class ForeignName
{
	public int Id { get; set; }
	public Guid CardId { get; set; }
	public string Language { get; set; } = string.Empty;
	public string Name { get; set; } = string.Empty;
	public string? Text { get; set; }
	public string? Flavor { get; set; }
	public int GathererId { get; set; }
}

public class Legality
{
	public int Id { get; set; }
	public Guid CardId { get; set; }
	public string Format { get; set; } = string.Empty;
	public string FormatLegality { get; set; } = string.Empty;
}
