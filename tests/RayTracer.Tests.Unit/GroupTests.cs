﻿using RayTracer.Models;
using RayTracer.Shapes;
using RayTracer.Extensions;

namespace RayTracer.Tests.Unit;

public class GroupTests
{
	[Fact]
	public void Constructor_ShouldInitializeEmptyCollection()
	{
		// Arrange
		// Act
		Group group = new();

		// Assert
		Assert.Empty(group);
	}

	[Fact]
	public void AddChild_ShouldSetParent_WhenShapeGiven()
	{
		// Arrange
		Group group = new();
		Shape shape = new TestShape();

		// Act
		group.AddChild(shape);

		// Assert
		Assert.Equal(group, shape.Parent);
	}

	[Fact]
	public void AddChild_ShouldAddShapeToCollection()
	{
		// Arrange
		Group group = new();
		Shape shape = new TestShape();
		const int expectedCount = 1;

		// Act
		group.AddChild(shape);

		// Assert
		Assert.Equal(expectedCount, group.Count);
		Assert.Contains(shape, group);
	}

	[Fact]
	public void Interesect_ShouldBeEmpty_WhenGroupEmpty()
	{
		// Arrange
		Group group = new();
		Ray ray = new(new Point(0, 0, 0), new Vector(0, 0, 1));

		// Act
		var result = group.Intersect(ray);

		// Assert
		Assert.Empty(result);
	}

	[Fact]
	public void Intersect_ShouldBeCorrect_WhenGroupNonEmpty()
	{
		// Arrange
		Group group = new();
		Sphere sphere1 = new();

		Matrix transform2 = Matrix.Translation(0, 0, -3);
		Sphere sphere2 = new(transform2);

		Matrix transform3 = Matrix.Translation(5, 0, 0);
		Sphere sphere3 = new(transform3);

		group.AddChild(sphere1);
		group.AddChild(sphere2);
		group.AddChild(sphere3);

		Ray ray = new(new Point(0, 0, -5), new Vector(0, 0, 1));
		const int expectedLength = 4;

		// Act
		var result = group.Intersect(ray);

		// Assert
		Assert.Equal(expectedLength, result.Length);
		Assert.Equal(sphere2, result[0].Object);
		Assert.Equal(sphere2, result[1].Object);
		Assert.Equal(sphere1, result[2].Object);
		Assert.Equal(sphere1, result[3].Object);
	}

	[Fact]
	public void Intersect_ShouldBeCorrect_WhenGroupAndShapeTransformed()
	{
		// Arrange
		Matrix groupTransform = Matrix.Scaling(2, 2, 2);
		Group group = new(groupTransform);
		Matrix childTransform = Matrix.Translation(5, 0, 0);
		Sphere child = new(childTransform);
		group.AddChild(child);

		Ray ray = new(new Point(10, 0, -10), new Vector(0, 0, 1));
		const int expectedLength = 2;

		// Act
		var result = group.Intersect(ray);

		// Assert
		Assert.Equal(expectedLength, result.Length);
	}

	[Fact]
	public void Intersect_ShouldNotTestChildren_WhenBoundingBoxMissed()
	{
		// Arrange
		TestShape child = new();
		Group group = new();
		group.AddChild(child);
		Ray ray = new(new(0, 0, -5), new(0, 1, 0));

		// Act
		group.Intersect(ray);

		// Assert
		Assert.Null(child.SavedLocalRay);
	}

	[Fact]
	public void Intersect_ShouldTestChildren_WhenBoundingBoxHit()
	{
        // Arrange
        TestShape child = new();
        Group group = new();
        group.AddChild(child);
        Ray ray = new(new(0, 0, -5), new(0, 0, 1));

        // Act
        group.Intersect(ray);

        // Assert
        Assert.NotNull(child.SavedLocalRay);
    }

	[Fact]
	public void BoundsOf_ShouldReturnBoundingBoxThatContainsChildren()
	{
		// Arrange
		Matrix sphereTransform = Matrix.Scaling(2, 2, 2).Translate(2, 5, -3);
		Sphere sphere = new(sphereTransform);
		Matrix cylinderTransform = Matrix.Scaling(0.5, 1, 0.5).Translate(-4, -1, 4);
		Cylinder cylinder = new(cylinderTransform)
		{
			Minimum = -2,
			Maximum = 2
		};
		Group group = new(new Shape[] { sphere, cylinder } );
		Point expectedMinimum = new(-4.5, -3, -5);
		Point expectedMaximum = new(4, 7, 4.5);

		// Act
		var result = group.BoundsOf();

		// Assert
		Assert.Equal(expectedMinimum, result.Minimum);
		Assert.Equal(expectedMaximum, result.Maximum);
	}

	[Fact]
	public void PartitionChildren_ShouldLeaveShapeInMiddleInGroup()
	{
		// Arrange
		Sphere sphere1 = new(Matrix.Translation(-2, 0, 0));
		Sphere sphere2 = new(Matrix.Translation(2, 0, 0));
		Sphere sphere3 = new();
		Group group = new(new Shape[] { sphere1, sphere2, sphere3 });

        // Act
		var (left, right) = group.PartitionChildren();

		// Assert
		Assert.Contains(sphere3, group);
		Assert.Contains(sphere1, left);
		Assert.Contains(sphere2, right);
    }

	[Fact]
	public void MakeSubGroup_ShouldAddNewGroup_WhenShapesGiven()
	{
		// Arrange
		Sphere sphere1 = new();
		Sphere sphere2 = new();
		Group group = new();

		// Act
		group.MakeSubgroup(new Shape[] { sphere1, sphere2 });

		// Assert
		Assert.True(group[0] is Group);
		Assert.Contains(sphere1, (Group)group[0]);
		Assert.Contains(sphere2, (Group)group[0]);
	}

	[Fact]
	public void Divide_ShouldNotDivide_WhenThresholdLessThanChildrenCount()
	{
        // Arrange
        Sphere sphere1 = new();
        Sphere sphere2 = new();
        Group group = new(new Shape[] { sphere1, sphere2 });
        const int threshold = 3;

        // Act
        group.Divide(threshold);

        // Assert
        Assert.Contains(sphere1, group);
        Assert.Contains(sphere2, group);
    }

	[Fact]
	public void Divide_ShouldDivide_WhenThresholdGreaterThanChildrenCount()
	{
        // Arrange
        Sphere sphere1 = new(Matrix.Translation(-2, -2, 0));
        Sphere sphere2 = new(Matrix.Translation(-2, 2, 0));
        Sphere sphere3 = new(Matrix.Scaling(4, 4, 4));
        Group group = new(new Shape[] { sphere1, sphere2, sphere3 });
        const int threshold = 1;

        // Act
        group.Divide(threshold);

		// Assert
		Assert.Equal(sphere3, group[0]);
		Assert.True(group[1] is Group);
		var subgroup = (Group)group[1];
        Assert.Contains(sphere1, (Group)subgroup[0]);
        Assert.Contains(sphere2, (Group)subgroup[1]);
    }

	[Fact]
	public void Divide_ShouldDivideChildren_WhenThresholdGreaterThanChildrenCount()
	{
        // Arrange
        Sphere sphere1 = new(Matrix.Translation(-2, 0, 0));
        Sphere sphere2 = new(Matrix.Translation(2, 1, 0));
        Sphere sphere3 = new(Matrix.Translation(2, -1, 0));
        Group subgroup = new(new Shape[] { sphere1, sphere2, sphere3 });
		Sphere sphere4 = new();
		Group group = new(new Shape[] { subgroup, sphere4 });
        const int threshold = 3;

        // Act
        group.Divide(threshold);

        // Assert
        Assert.Equal(subgroup, group[0]);
		Assert.Equal(sphere4, group[1]);
		Assert.Equal(2, subgroup.Count);
		Assert.True(subgroup[0] is Group);
		Assert.True(subgroup[1] is Group);
		var subgroup1 = (Group)subgroup[0];
		var subgroup2 = (Group)subgroup[1];
        Assert.Contains(sphere1, subgroup1);
        Assert.Contains(sphere2, subgroup2);
    }
}
