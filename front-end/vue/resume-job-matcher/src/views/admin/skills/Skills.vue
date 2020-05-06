<template>
  <div v-if="skills">
    <v-data-table
      :headers="headers"
      :items="skills"
      :expanded.sync="expanded"
      :search="search"
      item-key="id"
      :single-expand="true"
      show-expand
      class="elevation-2"
    >
      <!-- <v-data-table
      :headers="headers"
      :items="skills"
      :expanded.sync="expanded"
      :search="search"
      :custom-filter="customSkillsFilter"
      item-key="id"
      :single-expand="true"
      show-expand
      class="elevation-2"
    > -->
      <template v-slot:top>
        <v-toolbar flat>
          <v-toolbar-title>Skills</v-toolbar-title>
          <v-spacer></v-spacer>
          <v-text-field v-model="search" label="Search" single-line clearable hide-details></v-text-field>
        </v-toolbar>
      </template>
      <template v-slot:expanded-item="{ headers, item: skill }">
        <td :colspan="headers.length">
          <div>
            <v-combobox v-model="skill.aliases" @change="addSkillAlias(skill)" chips label="Aliases" multiple>
              <template v-slot:selection="{ attrs, item: alias, select, selected }">
                <v-chip v-bind="attrs" :input-value="selected" close @click="select" @click:close="removeSkillAlias(alias)">
                  {{ alias.name || alias }}
                </v-chip>
              </template>
            </v-combobox>
          </div>
        </td>
      </template>
    </v-data-table>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex';

export default {
  name: 'AdminSkills',
  data: () => ({
    expanded: [],
    search: null,
    headers: [
      {
        text: 'Name',
        align: 'start',
        sortable: true,
        value: 'displayName',
      },
      { text: 'Description', value: 'description' },
      { text: '', value: 'data-table-expand' },
    ],
  }),
  computed: {
    ...mapState('admin', {
      skills: (state) => state.skills,
    }),
    ...mapState('auth', {
      authenticated: (state) => state.authenticated,
      user: (state) => state.user,
    }),
    ...mapGetters('auth', {
      isAdmin: 'isAdmin',
      rolesList: 'rolesList',
    }),
  },
  mounted() {
    // Load skills
    this.$store.dispatch('admin/getAllSkills');
  },
  methods: {
    // customSkillsFilter(items, search, filter) {
    //   console.log(search, filter);

    //   if (!search) {
    //     return items;
    //   }

    //   search = search.toLowerCase();
    // },
    addSkillAlias(skill) {
      let alias;

      if (skill.aliases) {
        let aliasIndex = skill.aliases.findIndex((a) => !a.id);
        alias = skill.aliases.splice(aliasIndex, 1)[0];
      }

      if (!alias) {
        return;
      }

      this.$store.dispatch('admin/addSkillAliasToSkill', { skill, alias });
    },
    removeSkillAlias(alias) {
      this.$store.dispatch('admin/removeSkillAliasFromSkill', { alias });
    },
  },
};
</script>
