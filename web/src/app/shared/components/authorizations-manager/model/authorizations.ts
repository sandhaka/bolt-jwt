/**
 * Auth definition model retrieved from server
 */
export class AuthorizationDefinition {
  name: string;
  id: number;
  // Extra field to manage selection
  checked: boolean;
}

/**
 * Entity authorization relationship
 */
export class Authorization {
  entityAuthId: number;
  authId: number;
  name: string;
  // Field to manage selection
  checked: boolean;
}
