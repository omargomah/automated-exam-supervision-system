using API;
using FluentAssertions;
using NetArchTest.Rules;
using System.Reflection;

namespace Architecture
{
    public class ArchitectureTests
    {
        private const string ApplicationNamespace = "Application";
        private const string DomainNamespace = "Domain";
        private const string InfrastructureNamespace = "Infrastructure";
        private const string APINamespace = "API";



        [Fact]
        public void Domain_Should_HasNoDependencies()
        {
            // arrange
            Assembly assembly = typeof(Domain.AssemblyReference).Assembly;
            string[] otherProjectsNameSpaces = [ApplicationNamespace, InfrastructureNamespace, APINamespace];
            //act
            var result = Types.InAssembly(assembly).ShouldNot().HaveDependencyOnAny(otherProjectsNameSpaces).GetResult();

            //assert
            result.IsSuccessful.Should().BeTrue();

        }
        [Fact]
        public void Application_Should_HasNoDependencyOn_API_Infrastructure()
        {
            //arrange
            Assembly assembly = typeof(Application.AssemblyReference).Assembly;
            string[] otherProjectsNameSpaces = [InfrastructureNamespace,  APINamespace];

            //act
            var result = Types.InAssembly(assembly).ShouldNot().HaveDependencyOnAny(otherProjectsNameSpaces).GetResult();
            //assert
            result.IsSuccessful.Should().BeTrue();

        }
        [Fact]
        public void Infrastructure_Should_HasNoDependencyOn_API()
        {
            //arrange
            Assembly assembly = typeof(Infrastructure.AssemblyReference).Assembly;
            string[] otherProjectsNameSpaces = [APINamespace];

            //act
            var result = Types.InAssembly(assembly).ShouldNot().HaveDependencyOnAny(otherProjectsNameSpaces).GetResult();
            //assert
            result.IsSuccessful.Should().BeTrue();

        }
        [Fact]
        public void API_ProgramClass_ShouldHasDependencyOnApplicationInfrastructureAndNotInDomain()
        {
            //arrange
            Assembly assembly = typeof(API.AssemblyReference).Assembly;
            string[] otherProjectsNameSpacesDependOn = [InfrastructureNamespace, ApplicationNamespace];
            string[] otherProjectsNameSpacesNotDependOn = [DomainNamespace];
              
            //act
            var testResultOfDependency = Types.InAssembly(assembly)
            .That()
            .HaveName(nameof(Program)) 
            .Should()
            .HaveDependencyOnAll(otherProjectsNameSpacesDependOn)
            .GetResult();

            var testResultOfNotDependency = Types.InAssembly(assembly)
            .That()
            .HaveName(nameof(Program)) 
            .ShouldNot()
            .HaveDependencyOnAll(otherProjectsNameSpacesNotDependOn)
            .GetResult();

            //assert
            testResultOfDependency.IsSuccessful.Should().BeTrue();
            testResultOfNotDependency.IsSuccessful.Should().BeTrue();

        }

        [Fact]
        public void Controllers_Should_AlwaysDependOn_ApplicationAndDomain()
        {
            // Arrange
            var assembly = typeof(API.AssemblyReference).Assembly;
            string[] requiredNamespaces = [ApplicationNamespace, DomainNamespace];

            // Act - Filter only types in the Controllers namespace
            var result = Types.InAssembly(assembly)
                .That()
                .ResideInNamespaceContaining("Controllers")
                .Should()
                .HaveDependencyOnAll(requiredNamespaces)
                .GetResult();

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }

    }

}
