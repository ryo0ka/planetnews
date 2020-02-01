namespace NewsAPI.OfflineCopies
{
	// Share file/name format in reader & writer
	public static class NewsApiOfflineCopyFormat
	{
		public static string SourceFileName(string name) => $"{name}.sources.json";
		public static string ArticleFileName(string name, string postfix) => $"{name}_{postfix}.articles.json";
	}
}