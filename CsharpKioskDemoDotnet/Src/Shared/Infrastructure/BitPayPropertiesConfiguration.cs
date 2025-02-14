// Copyright 2023 BitPay.
// All rights reserved.

using CsharpKioskDemoDotnet.Shared.BitPayProperties;

using Microsoft.Extensions.Options;

using YamlDotNet.Serialization.NamingConventions;

namespace CsharpKioskDemoDotnet.Shared.Infrastructure;

public static class BitPayPropertiesConfiguration
{
    public static void Execute(WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.AddSingleton<IConfigureOptions<BitPayProperties.BitPayProperties>>(_ =>
        {
            var deserializer = new YamlDotNet.Serialization.DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .WithTypeMapping<IReadOnlyList<Field>, List<Field>>()
                .WithTypeMapping<IReadOnlyList<Option>, List<Option>>()
                .Build();
            var design = deserializer.Deserialize<Design>(File.ReadAllText("bitPayDesign.yaml"));

            return new ConfigureNamedOptions<BitPayProperties.BitPayProperties>(string.Empty, options =>
            {
                options.NotificationEmail = builder.Configuration.GetSection("BitPay:NotificationEmail").Value;
                options.Design = design;
            });
        });
    }
}