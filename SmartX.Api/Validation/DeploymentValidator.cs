using SmartX.Api.Models;

namespace SmartX.Api.Validation;

public class DeploymentValidator
{
    public List<string> GetValidationErrors(
        DeploymentNode node)
    {
        var errors = new List<string>();

        ValidateNode(node, "Root", errors);

        return errors;
    }

    public bool Validate(DeploymentNode node)
    {
        return GetValidationErrors(node).Count == 0;
    }

    private void ValidateNode(
        DeploymentNode node,
        string path,
        List<string> errors)
    {
        string currentPath =
            $"{path} > {node.Name}";

        if (!node.IsConfigured)
        {
            errors.Add(
                $"Unconfigured deployment node: {currentPath}");
        }

        foreach (var child in node.Children)
        {
            ValidateNode(
                child,
                currentPath,
                errors);
        }
    }
}