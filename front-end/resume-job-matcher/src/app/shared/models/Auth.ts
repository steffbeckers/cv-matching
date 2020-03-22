export class User {
  public Id: string;
  public FirstName: string;
  public LastName: string;
  public Username: string;
  public Email: string;
  public Roles: string[];
}

export class Login {
  public EmailOrUsername: string;
  public Password: string;
  public RememberMe: boolean;
}

export class Authenticated {
  public User: User;
  public Token: string;
  public RememberMe: boolean;
}
