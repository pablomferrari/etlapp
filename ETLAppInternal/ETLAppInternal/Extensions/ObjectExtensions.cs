using System.Collections.Generic;
using System.Linq;
using ETLAppInternal.Models.General;
using ETLAppInternal.Models.Materials;
using ETLAppInternal.Models.Samples;
using ETLAppInternal.Models.Sql;

namespace ETLAppInternal.Extensions
{
    public static class ObjectExtensions
    {
        public static IEnumerable<Mapping> ToMappingModel(this IEnumerable<MappingsTable> list)
        {
            return list.Select(x => new Mapping
            {
                // Id = x.Id,
                Classification = x.Classification,
                Friable = x.Friable,
                Material = x.Material,
                MaterialSub = x.MaterialSub,
                Units = x.Units
            });
        }

        public static IEnumerable<MappingsTable> ToMappingsTable(this IEnumerable<Mapping> list)
        {
            return list.Select(x => new MappingsTable
            {
                Classification = x.Classification,
                Friable = x.Friable,
                Material = x.Material,
                MaterialSub = x.MaterialSub,
                Units = x.Units
            });
        }

        public static MaterialsTable ToMaterialTable(this Materials arg)
        {
            return new MaterialsTable
            {
                Assumed = arg.Assumed,
                Classification = arg.Classification,
                ClientMaterialId = arg.ClientMaterialId,
                Color = arg.Color,
                Friable = arg.Friable,
                Id = arg.Id,
                JobId = arg.JobId,
                Location = arg.Location,
                Material = arg.Material,
                MaterialSub = arg.MaterialSub,
                Note1 = arg.Note1,
                Note2 = arg.Note2,
                Positive = arg.Positive,
                Quantity = arg.Quantity,
                Size = arg.Size,
                Units = arg.Units,
                IsLocal = arg.IsLocal,
                IsNew = arg.IsNew,
                CreatedDate = arg.CreatedDate,
                CreatedBy = arg.CreatedBy,
                EditedBy = arg.EditedBy,
                EditedDate = arg.EditedDate
            };
        }

        public static IEnumerable<MaterialsTable> ToMaterialsTable(this IEnumerable<Materials> list)
        {
            return list.Select(x => x.ToMaterialTable());
        }

        public static SamplesTable ToSamplesTable(this Samples sample)
        {
            return new SamplesTable
            {
                ClientSampleId = sample.ClientSampleId,
                DateCollected = sample.DateCollected,
                Id = sample.Id,
                JobId = sample.JobId,
                MaterialId = sample.MaterialId,
                SampleDescription = sample.SampleDescription,
                SampleLocation = sample.SampleLocation,
                IsLocal = sample.IsLocal,
                IsNew = sample.IsNew
            };
        }

        public static IEnumerable<DeliveryRequest> ToDeliveriesModel(this IEnumerable<DeliveryRequestTable> list)
        {
            return list.Select(x => x.ToDeliveryModel());
        }

        public static DeliveryRequest ToDeliveryModel(this DeliveryRequestTable table)
        {
            return new DeliveryRequest
            {
                EmployeeId = table.EmployeeId,
                JobId = table.JobId,
                StatusId = table.StatusId,
                TurnAround = table.TurnAround,
                PlmInstructions = table.PlmInstructions
            };
        }

        public static DeliveryRequestTable ToDeliveryRequestTable(this DeliveryRequest delivery)
        {
            return new DeliveryRequestTable
            {
                JobId = delivery.JobId,
                EmployeeId = delivery.EmployeeId,
                StatusId = 0,
                TurnAround = delivery.TurnAround,
                PlmInstructions = string.Join(";", delivery.PlmInstructions)
            };
        }

        public static IEnumerable<SamplesTable> ToSamplesTable(this IEnumerable<Samples> samples)
        {
            return samples.Select(x => x.ToSamplesTable());
        }

        public static Samples ToSample(this SamplesTable sample)
        {
            return new Samples
            {
                ClientSampleId = sample.ClientSampleId,
                DateCollected = sample.DateCollected,
                Id = sample.Id,
                JobId = sample.JobId,
                MaterialId = sample.MaterialId,
                SampleDescription = sample.SampleDescription,
                SampleLocation = sample.SampleLocation,
                IsLocal = sample.IsLocal,
                IsNew = sample.IsNew
            };
        }

        public static IEnumerable<Samples> ToSamples(this IEnumerable<SamplesTable> samples)
        {
            return samples.Select(x => x.ToSample());
        }

        public static Materials ToMaterial(this MaterialsTable material)
        {
            return new Materials
            {
                Assumed = material.Assumed,
                CanViewSamples = true,
                Classification = material.Classification,
                ClientMaterialId = material.ClientMaterialId,
                Color = material.Color,
                Friable = material.Friable,
                Id = material.Id,
                IsNew = material.IsNew,
                IsLocal = material.IsLocal,
                JobId = material.JobId,
                Location = material.Location,
                Material = material.Material,
                MaterialSub = material.MaterialSub,
                Note1 = material.Note1,
                Note2 = material.Note2,
                Positive = material.Positive,
                Quantity = material.Quantity,
                Size = material.Size,
                Units = material.Units,
            };
        }

        public static IEnumerable<Materials> ToMaterials(this IEnumerable<MaterialsTable> materials)
        {
            return materials.Select(x => x.ToMaterial());
        }
    }
}
