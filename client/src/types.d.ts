type Department = {
    id: string,
    title: string
}
type Service = {
    id: string,
    title: string,
    department_id : string,
    created_by : string,
    departmentTitle : string
}

type GroupOfSkills = {
    id: string,
    title: string
}

type Skills = {
    id: string,
    title : string,
    skills_group_id : string,
    skillsGroupTitle : string
}

type Employee = {
    id: string;
    lastName: string;
    firstName: string;
    entry_date: string;
    confirmation_date: string;
    service_id: string;
    serviceTitle: string;
    created_by: string;
};

type User = {
    id: string;
    email: string;
}

type Users = {
    id: string;
}

type LoginResponse = {
    token: string;
    user: {
        id: string;
        email: string;
    };
}

type UserContextType = {
    user: User | null;
    setUser: React.Dispatch<React.SetStateAction<User | null>>;
  }