import {Authorization} from "../../shared/components/authorizations-manager/model/authorizations";

export class UserDto {
  Id: number;
  Name: string;
  Surname: string;
  Email: string;
  Username: string;
  AuthorizationsList: Authorization[];
}
