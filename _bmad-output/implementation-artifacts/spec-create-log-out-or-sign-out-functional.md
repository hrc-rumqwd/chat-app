---
title: 'Create logout/sign-out functionality'
type: 'feature'
created: '2026-04-15'
status: 'draft'
context: ['{project-root}/_bmad-output/project-context.md']
---

<frozen-after-approval reason="human-owned intent — do not modify unless human renegotiates">

## Intent

**Problem:** Authenticated users can sign in but do not have a dedicated way to sign out, which makes session control incomplete and can leave shared-device sessions active longer than intended.

**Approach:** Add a logout endpoint in the existing auth controller that signs out the cookie-authenticated principal and redirects to login, then expose a sign-out action in the authenticated navbar so users can end sessions explicitly from any page.

## Boundaries & Constraints

**Always:** Reuse the existing cookie authentication setup; keep the implementation in web layer patterns already used by controllers and Razor views; show sign-out only when `User.Identity?.IsAuthenticated == true`; use POST for logout action to avoid accidental sign-out via crawlers or links.

**Ask First:** Changing cookie policy values (expiry, same-site, login path), introducing additional identity providers, or adding global middleware behaviors unrelated to logout.

**Never:** Modify login command behavior, redesign profile/admin navigation, or add client-side storage/session hacks to simulate sign-out.

## I/O & Edge-Case Matrix

| Scenario | Input / State | Expected Output / Behavior | Error Handling |
|----------|--------------|---------------------------|----------------|
| HAPPY_PATH | Authenticated user submits sign-out form | User cookie session is invalidated and response redirects to `/login` | N/A |
| ALREADY_SIGNED_OUT | Unauthenticated request hits logout endpoint | Request still redirects to `/login` without throwing | Ensure endpoint safely handles anonymous state |
| INVALID_METHOD | Browser or caller sends `GET /logout` | Route is not handled as logout action | Framework returns method/route mismatch; no sign-out side effect |

</frozen-after-approval>

## Code Map

- `ChatApp/Controllers/AuthController.cs` -- Existing auth routes (`/login`, `/sign-up`) where logout route should be added.
- `ChatApp/Views/Shared/_Layout.cshtml` -- Shared navbar for authenticated links; best place for sign-out button/form.
- `ChatApp/Program.cs` -- Cookie auth registration used by sign-out action.

## Tasks & Acceptance

**Execution:**
- [ ] `ChatApp/Controllers/AuthController.cs` -- Add POST logout action using `HttpContext.SignOutAsync` and redirect to login -- centralize sign-out behavior in current auth controller.
- [ ] `ChatApp/Views/Shared/_Layout.cshtml` -- Add authenticated-only logout form/button that posts to logout route -- provide discoverable user-facing sign-out control.
- [ ] `ChatApp/Controllers/AuthController.cs` -- Keep anonymous-safe behavior and anti-forgery expectation consistent with current app style -- prevent runtime errors for already-signed-out users.

**Acceptance Criteria:**
- Given an authenticated user, when they click sign out from the navbar, then they are redirected to `/login` and no longer have authenticated access.
- Given a user who has signed out, when they navigate to a protected page like `/profile`, then they are redirected to `/login`.
- Given an anonymous request to the logout POST endpoint, when it executes, then it completes safely and redirects to `/login`.

## Spec Change Log

## Verification

**Commands:**
- `dotnet build` -- expected: build succeeds with no new errors.

**Manual checks (if no CLI):**
- Login with a valid account, click sign out from navbar, verify redirect to login.
- After sign-out, browse to `/profile`, verify redirect to login.
