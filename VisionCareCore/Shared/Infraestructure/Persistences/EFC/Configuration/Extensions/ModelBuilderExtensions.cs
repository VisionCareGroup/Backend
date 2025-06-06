﻿using Microsoft.EntityFrameworkCore;

namespace ProjectCalculadoraAMSAC.Shared.Infraestructure.Persistences.EFC.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void UseSnakeCaseNamingConvention(this ModelBuilder builder)
    {
        foreach (var entity in builder.Model.GetEntityTypes())
        {
            var tableName = entity.GetTableName();
            if (!string.IsNullOrEmpty(tableName)) 
                entity.SetTableName(tableName.ToSnakeCase());
            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(property.GetColumnName().ToSnakeCase());
            }

            foreach (var key in entity.GetKeys())
            {
                var keyName = key.GetName();
                if (!string.IsNullOrEmpty(keyName)) entity.SetTableName(keyName.ToSnakeCase());
            }

            foreach (var foreignKey in entity.GetForeignKeys())
            {
                var foreignKeyConstraintName = foreignKey.GetConstraintName();
                if (!string.IsNullOrEmpty(foreignKeyConstraintName)) 
                    entity.SetTableName(foreignKeyConstraintName.ToSnakeCase());
            }
            
            foreach (var index in entity.GetIndexes())
            {
                var indexDatabaseName = index.GetDatabaseName();
                if (!string.IsNullOrEmpty(indexDatabaseName)) 
                    entity.SetTableName(indexDatabaseName.ToSnakeCase());
            }
        }
    }
}