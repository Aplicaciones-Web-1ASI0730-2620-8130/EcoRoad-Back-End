# Identity and Access Management (IAM)

The IAM bounded context authenticates the collaborators of each company, issues their
session tokens and governs their roles, permissions and access. Other bounded contexts only
reference users by identifier and ask IAM questions through `IIamContextFacade`.

## Domain model

| Element | Type | Responsibility |
| --- | --- | --- |
| `User` | Aggregate root | Collaborator of a company. Owns its role, its effective permissions and its access status. |
| `UserInvitation` | Aggregate root | Invitation sent by an administrator. Accepting it generates a `User` with pending access. |
| `Role` | Value object (enum) | `CompanyAdmin`, `SiteDirector`, `EnvironmentalEngineer`, `Inspector`, `FieldCollaborator`. |
| `Permission` | Value object (enum) | Fine grained permissions such as `ManageIncidents` or `ManageUsers`. |
| `RolePermissions` | Value object catalog | Default permissions granted by every role. |
| `AccessStatus` | Value object (enum) | `Pending`, `Active`, `Revoked`. |
| `InvitationStatus` | Value object (enum) | `Pending`, `Accepted`, `Expired`. |
| `AccessManagementPolicy` | Domain service | Rules that involve the acting administrator and the target collaborator. |

## Business rules

- The first user of a company signs up as `CompanyAdmin` with active access. Later users join through invitations.
- Passwords need at least 8 characters with upper case letters, lower case letters and digits. Only BCrypt hashes are stored.
- An invitation is valid for 7 days and can be accepted only once. Only one pending invitation per email and company is allowed.
- An accepted invitation creates the user with `Pending` access; the user can sign in only after an administrator grants access.
- Only active users with the `ManageUsers` permission can invite collaborators or change their access, and only inside their own company.
- Administrators cannot revoke their own access, remove their own administrator role or remove their own `ManageUsers` permission.
- Assigning a role resets the permissions to the defaults of the role. Permissions can be edited only while the access is active.
- Sessions are JSON Web Tokens signed with HMAC SHA-256. Every request reloads the user, so revoking access invalidates existing tokens.

## Endpoints

| Method | Route | Description |
| --- | --- | --- |
| POST | `/api/iam/sign-up` | Registers the first administrator of a company. |
| POST | `/api/iam/sessions` | Signs in and returns `{ token, user }`. |
| GET | `/api/iam/sessions/current` | Returns the user of the current session. |
| DELETE | `/api/iam/sessions/current` | Ends the session on the client. |
| GET | `/api/iam/users` | Lists the collaborators of the company. |
| GET | `/api/iam/users/{userId}` | Returns a collaborator. |
| POST | `/api/iam/users/{userId}/grant` | Grants access. |
| POST | `/api/iam/users/{userId}/revoke` | Revokes access. |
| PUT | `/api/iam/users/{userId}/role` | Assigns a role, body `{ role }`. |
| PUT | `/api/iam/users/{userId}/permissions` | Replaces the permissions, body `{ permissions }`. |
| GET | `/api/iam/invitations?status=` | Lists the invitations of the company. |
| POST | `/api/iam/invitations` | Invites a collaborator, body `{ name, email, role }`. |
| GET | `/api/iam/invitations/{invitationId}` | Returns an invitation (anonymous). |
| POST | `/api/iam/invitations/{invitationId}/acceptance` | Accepts an invitation, body `{ password }` (anonymous). |

Protected endpoints require the header `Authorization: Bearer <token>`. Errors are returned as
`{ code, message }` with status `401`, `403`, `404`, `409` or `422`.

## Persistence

Tables `users` and `user_invitations`, created by the migration `InitialIam`. Enumerations are
stored as text and the permissions as a comma separated list.

## Local demo data

In the `Development` environment the company `demo-company` is seeded with the administrator
`c.mendoza@empresa.pe` and three collaborators. Every demo user signs in with `EcoRoadDemo123!`.
Set a real `TokenSettings:Secret` (32 characters or more) outside development.
