﻿/* Copyright 2010-present MongoDB Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MongoDB.Driver.Tests.Linq.Linq3ImplementationTests.Jira
{
    public class CSharp4047Tests : Linq3IntegrationTest
    {
        [Fact]
        public void Nested_where_should_work_with_array()
        {
            var collection = GetCollection<C>();

            var queryable = collection.AsQueryable()
                .Select(x => new { R = x.A.Where(i => i == 1)});

            var stages = Translate(collection, queryable);
            var expectedStages = new[]
            {
                "{ $project : { R : { $filter : { input : '$A', as : 'i', cond : { $eq : ['$$i', 1] } } }, _id : 0 } }"
            };
            AssertStages(stages, expectedStages);
        }

        [Fact]
        public void Nested_where_should_work_with_IEnumerable()
        {
            var collection = GetCollection<C>();

            var queryable = collection.AsQueryable()
                .Select(x => new { R = x.E.Where(i => i == 1) });

            var stages = Translate(collection, queryable);
            var expectedStages = new[]
            {
                "{ $project : { R : { $filter : { input : '$E', as : 'i', cond : { $eq : ['$$i', 1] } } }, _id : 0 } }"
            };
            AssertStages(stages, expectedStages);
        }

        [Fact]
        public void Nested_where_should_work_with_List()
        {
            var collection = GetCollection<C>();

            var queryable = collection.AsQueryable()
                .Select(x => new { R = x.L.Where(i => i == 1) });

            var stages = Translate(collection, queryable);
            var expectedStages = new[]
            {
                "{ $project : { R : { $filter : { input : '$L', as : 'i', cond : { $eq : ['$$i', 1] } } }, _id : 0 } }"
            };
            AssertStages(stages, expectedStages);
        }

        private class C
        {
            public int Id { get; set; }
            public int[] A { get; set; }
            public IEnumerable<int> E { get; set; }
            public List<int> L { get; set; }
        }
    }
}
