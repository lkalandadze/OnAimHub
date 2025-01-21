# GitLab Repository Guide

## Introduction
Welcome to the GitLab repository for the OnAim project. This document provides essential information about working with Kubernetes, the branch strategy for CI/CD pipelines, and how to interact with the repository effectively.

---

## Kubernetes Basics for Developers
Kubernetes (K8s) is a platform for managing containerized applications. Below are the key commands developers need to interact with the Kubernetes cluster.

### Viewing Resources
- **List all Pods in a namespace**:
  ```bash
  kubectl get pods -n <namespace>
  ```
- **List all Deployments in a namespace**:
  ```bash
  kubectl get deployments -n <namespace>
  ```
- **List all Services in a namespace**:
  ```bash
  kubectl get services -n <namespace>
  ```

### Troubleshooting
- **Describe a Pod**:
  ```bash
  kubectl describe pod <pod-name> -n <namespace>
  ```
- **View Logs of a Pod**:
  ```bash
  kubectl logs <pod-name> -n <namespace>
  kubectl logs <pod-name> -f
  ```
- **Execute a Command in a Pod**:
  ```bash
  kubectl exec -it <pod-name>  -- sh
  ```

### Managing Resources
- **Scale a Deployment**:
  ```bash
  kubectl scale deployment <deployment-name> --replicas=<number> -n <namespace>
  ```
- **Apply a Configuration File**:
  ```bash
  kubectl apply -f <file.yaml> -n <namespace>
  ```
- **Delete a Resource**:
  ```bash
  kubectl delete <resource-type> <resource-name> -n <namespace>
  ```

---

## GitLab Branch Strategy
The repository uses a structured branch naming convention for CI/CD pipelines. Below is the mapping of branches to their respective components:

| **Component**                         | **Branch Regex**                  |
|---------------------------------------|------------------------------------|
| **Admin/OnAim.Admin.API**             | `^admin\/.*`                     |
| **ApiGateways/ApiGateway**            | `^api_gateway\/.*`               |
| **Core/Hub**                          | `^hub_api\/.*`                   |
| **CoreModules/Leaderboard**           | `^leaderboard\/.*`               |
| **CoreModules/LevelService**          | `^levelservice\/.*`              |
| **CoreModules/MissionService**        | `^missionservice\/.*`            |
| **CoreModules/AggregationService**    | `^aggregationservice\/.*`        |
| **Games/Wheel**                       | `^wheel\/.*`                     |
| **Games/PenaltyKicks**                | `^penaltykicks\/.*`              |
| **Helpers/Test.Api**                  | `^testapi\/.*`                   |
| **StateMachines/SagaOrchestration**   | `^sagamachine\/.*`               |

### How It Works
- Each branch corresponds to a specific module or component.
- CI/CD pipelines are triggered based on branch names using the patterns defined above.
- Developers must create branches using the appropriate naming convention for their component.

### Example
If you are working on the **Leaderboard** module, create a branch like this:
```
leaderboard/feature-xyz
```

The pipeline will automatically detect and run the CI/CD jobs for the `leaderboard` component.

---

## Workflow Guidelines
1. **Branch Naming**:
   - Follow the branch naming conventions strictly to ensure pipelines trigger correctly.
   - Example: `admin/bugfix-123`, `api_gateway/feature-xyz`.

2. **Merge Requests**:
   - Create a merge request for every branch to ensure proper review and testing.

3. **Pipeline Monitoring**:
   - Check the status of pipelines in GitLab for your branch.
   - Fix any failing jobs before merging.

---

## Swagger Hosts
Below are the Swagger URLs for the various services in the OnAim project:

| **Service**                         | **Swagger URL**                                                 |
|-------------------------------------|-----------------------------------------------------------------|
| Aggregation Service                 | `http://onaim.aggregation.dev.local:30001/swagger/index.html`         |
| Admin API                           | `http://onaim.adminapi.dev.local:30001/swagger/index.html`      |
| Ocelot API Gateway                  | `http://onaim.ocelotapigateway.dev.local:30001/swagger/index.html`    |
| Hub API                             | `http://onaim.hubapi.dev.local:30001/swagger/index.html`              |
| Leaderboard API                     | `http://onaim.leaderboardapi.dev.local:30001/swagger/index.html`      |
| Level Service                       | `http://onaim.levelapi.dev.local:30001/swagger/index.html`            |
| Mission Service                     | `http://onaim.missionapi.dev.local:30001/swagger/index.html`          |
| Wheel API                           | `http://onaim.wheelapi.dev.local:30001/swagger/index.html`            |
| Test API                            | `http://onaim.testapi.dev.local:30001/swagger/index.html`             |
| Saga Orchestration State Machine    | `http://onaim.sagamachine.dev.local:30001/swagger/index.html`         |
| Penalty Kicks API                   | `http://onaim.penaltykicksapi.dev.local:30001/swagger/index.html`     |

---

## Additional Resources
- [Kubernetes Documentation](https://kubernetes.io/docs/)
- [GitLab CI/CD Documentation](https://docs.gitlab.com/ee/ci/)

---

Feel free to reach out to the DevOps team for any questions or assistance!
