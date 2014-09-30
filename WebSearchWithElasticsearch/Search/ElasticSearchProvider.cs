﻿using System;
using System.Collections.Generic;
using ElasticLinq;
using ElasticsearchCRUD;

namespace WebSearchWithElasticsearch.Search
{
	public class ElasticSearchProvider : ISearchProvider
	{
		private const string ConnectionString = "http://localhost:9200";
		private readonly IElasticSearchMappingResolver _elasticSearchMappingResolver = new ElasticSearchMappingResolver();

		public IEnumerable<Skill> QueryString(string term)
		{
			var connection = new ElasticConnection(new Uri(ConnectionString));
			var context = new ElasticContext(connection);

			if (term != null)
			{
				var names = term.Replace("+", " OR *");

				return context.Query<Skill>().QueryString(names + "*");
			}

			return context.Query<Skill>().QueryString("*");
		}

		public void AddUpdateEntity(Skill skill)
		{
			using (var context = new ElasticSearchContext(ConnectionString, _elasticSearchMappingResolver))
			{
				context.AddUpdateEntity(skill, skill.Id);
				context.SaveChanges();
			}
		}
	}
}